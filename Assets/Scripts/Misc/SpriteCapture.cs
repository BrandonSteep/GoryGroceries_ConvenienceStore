using UnityEngine;
using System.IO;


public class SpriteCapture : MonoBehaviour
{

    //  Copyright 2021 Mordred CiarDreki

    //  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
    //  documentation files(the "Software"), to deal in the Software without restriction, including without limitation
    //  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
    //  and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

    //  The above copyright notice and this permission notice shall be included in all copies or substantial portions
    //  of the Software.

    //  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
    //  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL
    //  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
    //  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
    //  DEALINGS IN THE SOFTWARE.

    //  INSTRUCTIONS: PLEASE READ CAREFULLY.

    /* TO WORK:
     *          1: ATTACH SCRIPT TO GAMEOBJECT YOU WISH TO CAPTURE ON YOUR SCENE.
     *          2: SUPPLY
     *              2a: FILE NAME.
     *              2b: FOLDER NAME.
     *              2d: CAMERA SIZE.
     *          3: ADJUST ANY OTHER SETTINGS
     *          4: PLAY SCENE
     *          5: PRESS NUMPAD 0 TO BEGIN CAPTURE
    */

    public enum CaptureType { Animation, SingleShot, Rotation }

    [Tooltip("What Capture type do you want?\n\nAnimation - Capture a full animation from a single angle.\nSingle Shot - Take a single PNG of the Object from a single angle.\nRotation - Rotate 360 degrees around the Object, taking a single PNG every angle.")]
    public CaptureType captureType = CaptureType.SingleShot;

    public CameraClearFlags cameraClearFlag = CameraClearFlags.SolidColor; // The Clear Flag for the Camera.

    public string fileName = ""; // Name of the files

    public string folder = ""; // Folder Name you want the sprites saved to

    public int framesToCapture = 30; // Frames to Capture


    public float cameraSize; // Size of the camera

    public int millisecondsToWaitBeforeStart = 0; // Length of time before we begin capturing.

    // Camera Positions
    public float PosX = 0.0f;
    public float PosY = 0.0f;
    public float PosZ = -10.0f;

    // Camera Rotations
    public float RotX = 0.0f;
    public float RotY = 0.0f;
    public float RotZ = 0.0f;


    // White Camera
    private Camera whiteCam;

    // Black Camera
    private Camera blackCam;

    private float originalTime; // The original time so we can stop the frames.

    private string defaultFolder = "Sprites - Animations"; // Default Folder

    private bool done = false; // Is the routine done?

    private Texture2D textureBlack; // black camera texture

    private Texture2D textureWhite; // white camera texture

    private Texture2D outputTexture; // final output texture

    private RenderTexture blackCamRenderTexture; // Black camera render

    private RenderTexture whiteCamRenderTexture; // White camera render

    private int imagesTaken = 0; // How many images have we taken total.

    private bool process = false; // Are we still processing?

    public void Start()
    {

        if (!string.IsNullOrEmpty(folder))
            defaultFolder = folder; // Set's the folder name, otherwise uses the default.

        if (!Directory.Exists(defaultFolder)) //Check if the folder already exists, if not creates it.
        {
            Directory.CreateDirectory(defaultFolder);
        }

        originalTime = Time.timeScale; // Gets the time so we can stop it later.

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0)) // Checks to see if Numpad 0 is pressed.
        {
            blackSetup(); // Sets up Black Camera
            whiteSetup(); // Sets up White Camera
            process = true; // Set the bool so we know we're processing sprites.
        }

        if (process)
        {
            Capture();
        }
    }

    void LateUpdate()
    {
        // When we are all done capturing, clean up the cameras and remove everything from the the scene.
        if (done)
        {
            DestroyImmediate(textureBlack);
            DestroyImmediate(textureWhite);
            DestroyImmediate(outputTexture);

            whiteCam.targetTexture = null;
            RenderTexture.active = null;
            DestroyImmediate(whiteCamRenderTexture);

            blackCam.targetTexture = null;
            RenderTexture.active = null;
            DestroyImmediate(blackCamRenderTexture);
            DestroyImmediate(GameObject.Find("Black Camera"));
            DestroyImmediate(GameObject.Find("White Camera"));
            done = false;
        }
    }

    private void blackSetup()
    {

        GameObject bc = new GameObject("Black Camera");
        bc.transform.localPosition = new Vector3(PosX, PosY, PosZ);
        bc.transform.Rotate(new Vector3(RotX, RotY, RotZ));
        blackCam = bc.AddComponent<Camera>();
        blackCam.backgroundColor = Color.black;
        blackCam.clearFlags = cameraClearFlag;
        blackCam.orthographic = true;
        blackCam.orthographicSize = cameraSize;
    }

    private void whiteSetup()
    {

        GameObject wc = new GameObject("White Camera");
        wc.transform.localPosition = new Vector3(PosX, PosY, PosZ);
        wc.transform.Rotate(new Vector3(RotX, RotY, RotZ));
        whiteCam = wc.AddComponent<Camera>();
        whiteCam.backgroundColor = Color.white;
        whiteCam.clearFlags = cameraClearFlag;
        whiteCam.orthographic = true;
        whiteCam.orthographicSize = cameraSize;
    }


    private void Capture()
    {
        blackCam.transform.LookAt(transform);
        whiteCam.transform.LookAt(transform);

        // Stop time
        Time.timeScale = 0;

        //Initialize and render the textures
        blackCamRenderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        whiteCamRenderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);

        // Render the 2D Texture for Black Camera
        blackCam.targetTexture = blackCamRenderTexture;
        blackCam.Render();
        RenderTexture.active = blackCamRenderTexture;
        textureBlack = GetTexture2D(true);

        // Render the 2D Texture for White Camera
        whiteCam.targetTexture = whiteCamRenderTexture;
        whiteCam.Render();
        RenderTexture.active = whiteCamRenderTexture;
        textureWhite = GetTexture2D(true);


        // Make sure we have both 2D Textures to create the final Output
        if (textureWhite && textureBlack)
        {

            int width = Screen.width;
            int height = Screen.height;

            outputTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);

            // Create Alpha from the difference between black and white camera renders
            for (int y = 0; y < outputTexture.height; ++y)
            {
                for (int x = 0; x < outputTexture.width; ++x)
                {
                    float alpha;

                    alpha = textureWhite.GetPixel(x, y).r - textureBlack.GetPixel(x, y).r;

                    alpha = 1.0f - alpha;
                    Color color = alpha < 0.01f ? Color.clear : textureBlack.GetPixel(x, y) / alpha;
                    color.a = alpha;
                    outputTexture.SetPixel(x, y, color);
                }
            }

            byte[] pngShot = outputTexture.EncodeToPNG();
            File.WriteAllBytes(string.Format("{0}/" + fileName + "{1:D04}.png", defaultFolder, imagesTaken), pngShot);

            imagesTaken++;

            switch (captureType)
            {
                case CaptureType.Animation:
                    if (imagesTaken == framesToCapture)
                    {
                        process = false;
                        done = true;
                    }
                    break;
                case CaptureType.SingleShot:
                    process = false;
                    done = true;
                    break;
                case CaptureType.Rotation:
                    blackCam.transform.Translate(Vector3.right * Time.deltaTime);
                    whiteCam.transform.Translate(Vector3.right * Time.deltaTime);
                    if (imagesTaken == framesToCapture)
                    {
                        process = false;
                        done = true;
                    }
                    break;
            }

            Time.timeScale = originalTime;

        }
        else
        {
            Debug.LogError("Error:  unable to render, one or both textures failed to render.");
        }
    }

    private Texture2D GetTexture2D(bool renderAll)
    {
        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        if (!renderAll)
        {
            width = width / 2;
        }

        Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
        // Read screen contents into the texture
        texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture2D.Apply();
        return texture2D;
    }
}
