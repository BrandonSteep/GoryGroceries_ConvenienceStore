/*
 * Morose Labs - Core Library
 * MoroseLabs@Outlook.com
*/

using System.IO;
using UnityEngine;

namespace MoroseLabs.Tools
{
    #region Licensing
    // Copyright 2021 Mordred CiarDreki

    // Copyright 2024 Morose Labs

    // Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
    // documentation files(the "Software"), to deal in the Software without restriction, including without limitation
    // the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software,
    // and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

    // The above copyright notice and this permission notice shall be included in all copies or substantial portions
    // of the Software.

    // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
    // TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL
    // THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
    // CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
    // DEALINGS IN THE SOFTWARE.
    #endregion

    #region Instructions
    // 1.) Add the script to the gameobject you wish to capture.
    // 2.) Adjust your settings and provide a file name.
    // 3.) If you wish to provide a directory you may do so, otherwise it will create a directory named "Sprites".
    // 4.) In 'Play' mode press C to execute the capture.
    #endregion

    /// <summary>
    /// Tool for creating sprites of gameobjects within Unity.
    /// Includes support for animations and rotations.
    /// </summary>
    [AddComponentMenu("Morose Labs/Tools/Sprite Capture Tool")]
    public sealed class SpriteCaptureTool : MonoBehaviour
    {
        #region File and Directory Settings
        [Header("File and Directory Settings")]
        [Tooltip("The name of the sprite file that will be produced.")]
        [SerializeField] string _fileName = "";

        [Tooltip("The location the file will be saved to.")]
        [SerializeField] string _directory = "";
        #endregion

        #region Capture Settings
        [Header("Capture Settings")]
        // [SerializeField] CameraClearFlags _cameraClearFlag = CameraClearFlags.SolidColor;

        [Tooltip("What Capture type do you want?" +
            "\nAnimation - Capture a full animation from a single angle." +
            "\nSingle Shot - Take a single PNG of the Object from a single angle." +
            "\nRotation - Rotate 360 degrees around the Object, taking a single PNG every angle.")]
        [SerializeField] CaptureType _captureType = CaptureType.Single;

        [Tooltip("The amount of shots that will be taken.")]
        [SerializeField] int _framesToCapture = 1;

        [Tooltip("The size of the camera. (Default is 1)")]
        [SerializeField] float _cameraSize = 1.0f;

        [Tooltip("The camera's position, relative to the gameobject.")]
        [SerializeField] Vector3 _cameraPosition = new Vector3(0, 0, -3.0f);

        [Tooltip("The camera's rotation, relative to the gameobject.")]
        [SerializeField] Vector3 _cameraRotation = Vector3.zero;
        #endregion

        #region Private Properties
        private Camera _camera1;
        private Camera _camera2;
        private Texture2D _texture1;
        private Texture2D _texture2;
        private RenderTexture _renderTexture1;
        private RenderTexture _renderTexture2;
        private Texture2D _ouputTexture;
        private int _shotCount = 0;
        private float _captureTimeDelta;
        private string _defaultDirectory = "Sprites";
        private bool _processing = false;
        private bool _completed = false;
        #endregion

        private void Start()
        {
            CreateDirectory();
            _captureTimeDelta = Time.timeScale;
        }

        private void Update()
        {
            if (!_processing && Input.GetKeyDown(KeyCode.C))
            {
                InitializeCameras();
                _processing = true;
            }

            if (_processing)
            {
                Capture();
            }
        }

        private void LateUpdate()
        {
            if (_processing && _captureType == CaptureType.Rotation)
            {
                RotateCameras();
            }

            if (_completed)
            {
                CleanUp();
            }
        }

        private void CreateDirectory()
        {
            if (!string.IsNullOrEmpty(_directory))
            {
                _defaultDirectory = _directory;
            }

            if (!Directory.Exists(_defaultDirectory))
            {
                Directory.CreateDirectory(_defaultDirectory);
            }
        }

        private void InitializeCameras()
        {
            // Camera 1
            GameObject camera1 = new GameObject("SpriteCapture Camera 1");
            camera1.transform.localPosition = _cameraPosition;
            camera1.transform.Rotate(_cameraRotation);
            _camera1 = camera1.AddComponent<Camera>();
            _camera1.backgroundColor = Color.white;
            _camera1.clearFlags = CameraClearFlags.SolidColor;
            _camera1.orthographic = true;
            _camera1.orthographicSize = _cameraSize;

            // Camera 2
            GameObject camera2 = new GameObject("SpriteCapture Camera 2");
            camera2.transform.localPosition = _cameraPosition;
            camera2.transform.Rotate(_cameraRotation);
            _camera2 = camera2.AddComponent<Camera>();
            _camera2.backgroundColor = Color.black;
            _camera2.clearFlags = CameraClearFlags.SolidColor;
            _camera2.orthographic = true;
            _camera2.orthographicSize = _cameraSize;
        }

        private void Capture()
        {
            _camera1.transform.LookAt(transform);
            _camera2.transform.LookAt(transform);

            Time.timeScale = 0;

            RenderCameras();

            if (_texture1 && _texture2)
            {
                int width = Screen.width;
                int height = Screen.height;
                _ouputTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);

                CreateAlpha();
                WriteCapturedShot();
                SetCaptureStatus();
                
                Time.timeScale = _captureTimeDelta;
            }
            else
            {
                Debug.LogError("Unable to render, one or both textures failed to render.");
            }
        }

        private void RenderCameras()
        {
            // Camera 1
            _renderTexture1 = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            _camera1.targetTexture = _renderTexture1;
            _camera1.Render();
            RenderTexture.active = _renderTexture1;
            _texture1 = GetTexture2D(true);

            // Camera 2
            _renderTexture2 = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
            _camera2.targetTexture = _renderTexture2;
            _camera2.Render();
            RenderTexture.active = _renderTexture2;
            _texture2 = GetTexture2D(true);
        }

        private void CreateAlpha()
        {
            for (int y = 0; y < _ouputTexture.height; ++y)
            {
                for (int x = 0; x < _ouputTexture.width; ++x)
                {
                    float alpha;
                    alpha = _texture1.GetPixel(x, y).r - _texture2.GetPixel(x, y).r;

                    alpha = 1.0f - alpha;
                    Color color = alpha < 0.01f ? Color.clear : _texture2.GetPixel(x, y) / alpha;
                    color.a = alpha;
                    _ouputTexture.SetPixel(x, y, color);
                }
            }
        }

        private void WriteCapturedShot()
        {
            byte[] pngShot = _ouputTexture.EncodeToPNG();
            File.WriteAllBytes(string.Format("{0}/" + _fileName + "{1:D04}.png", _defaultDirectory, _shotCount), pngShot);
            _shotCount++;
        }

        private void SetCaptureStatus()
        {
            if (_captureType == CaptureType.Single)
            {
                _processing = false;
                _completed = true;
            }
            else if (_captureType == CaptureType.Animation)
            {
                if (_shotCount == _framesToCapture)
                {
                    _processing = false;
                    _completed = true;
                }
            }
            else if (_captureType == CaptureType.Rotation)
            {
                if (_shotCount == _framesToCapture)
                {
                    _processing = false;
                    _completed = true;
                }
            }

            if (_completed)
            {
                Debug.Log("Successfully captured " + _captureType + " of " + _fileName);
            }
        }

        private void RotateCameras()
        {
            _camera1.transform.Translate(Vector3.right * Time.deltaTime);
            _camera2.transform.Translate(Vector3.right * Time.deltaTime);
        }

        private void CleanUp()
        {
            DestroyImmediate(_texture1);
            DestroyImmediate(_texture2);
            DestroyImmediate(_ouputTexture);

            _camera1.targetTexture = null;
            RenderTexture.active = null;
            DestroyImmediate(_renderTexture1);

            _camera2.targetTexture = null;
            RenderTexture.active = null;
            DestroyImmediate(_renderTexture2);

            DestroyImmediate(GameObject.Find("SpriteCapture Camera 1"));
            DestroyImmediate(GameObject.Find("SpriteCapture Camera 2"));
            _completed = false;
        }

        private Texture2D GetTexture2D(bool renderAll)
        {
            int width = Screen.width;
            int height = Screen.height;
            if (!renderAll)
            {
                width = width / 2;
            }

            Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
            texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture2D.Apply();
            return texture2D;
        }
    }

    internal enum CaptureType
    {
        Single,
        Animation,
        Rotation
    }
}