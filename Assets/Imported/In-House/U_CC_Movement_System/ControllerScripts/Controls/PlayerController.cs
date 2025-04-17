using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // VARIABLES //
    #region Variables
    // INPUT //
    [Header ("Input")]
    [SerializeField]
    CharacterController controller = null;
    Vector2 horizontalInput;
    public Vector2 mouseLookInput;
    public float running;
    public float aiming;



    // M O V E M E N T //
    // MISC //
    [SerializeField] [Tooltip("Default: Default 2.75")] float walkSpeed = 2.75f;    
    [SerializeField] [Tooltip("Default: Default 0.75")] float aimWalkSpeedMultiplier = 0.75f;
    [SerializeField] float currentWalkSpeed;

    // RUNNING //
    [Header("Running")]
    [SerializeField] float runSpeedMultiplier = 2.0f;
    private bool canRun = true;
    [SerializeField] [Tooltip("Default: 100")] ScriptableVariable currentStamina;
    [SerializeField] [Tooltip("Default: 100")] ScriptableVariable maxStamina;
    [SerializeField] [Tooltip("Default: 15")] float staminaDrainRate = 20f;
    [SerializeField] [Tooltip("Default: 20")] float staminaRefillRate = 20f;

    // SLOPE HANDLING //
    [Header("Slope Handling")]
    [SerializeField] private float slopeForce = 100f;
    [SerializeField] private float slopeForceRayLength;
    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    // M O U S E   L O O K //
    private Transform playerCamera = null;
    [SerializeField] float mouseSensitivity = 3.5f;
    //[SerializeField] float aimSpeed = 2.0f;

    [SerializeField] private float playerFov = 75f;
    [SerializeField] private float fovZoomAmount = 55;
    [SerializeField] private float fovZoomTime = 0.5f;
    private bool fovZoomActive = false;
    private bool currentlyZooming = false;

    [SerializeField] bool lockCursor = true;

    float cameraPitch = 0.0f;
    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;
    private CameraShake shake;


    // GRAVITY //
    [Header("Physics")]
    [SerializeField] float gravity = -13.0f;
    float velocityY = 0.0f;

    // SMOOTHING //
    [Header ("Smoothing")]
    [SerializeField] [Range(0.0f, 0.5f)] float moveSmoothTime = 0.15f;
    [SerializeField] [Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    #endregion



    // FRAME-BASED ///
    #region Frame-Based Methods
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = ControllerReferences.cam.transform;
        shake = GetComponent<CameraShake>();
        Camera.main.fieldOfView = playerFov;
        
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        currentWalkSpeed = walkSpeed; // Add Item-Based Walk Speed Adjustment Here

        currentStamina.value = maxStamina.value;
    }

    void FixedUpdate()
    {
        UpdateGravity();
        UpdateStamina();
        
        if(controller.enabled){
            UpdateMovement();
        }

        //Debug.Log("stamina = " + stamina);
    }

    void Update(){
        if(controller.enabled){
            UpdateMouseLook();
        }
    }
    #endregion



    // MOVEMENT //
    #region Keyboard & Mouse Controls

    void UpdateMovement()
    {
        Vector2 targetDir = horizontalInput;
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        if (aiming == 1f)
        {
            currentWalkSpeed *= aimWalkSpeedMultiplier;
            if(!fovZoomActive && !currentlyZooming){
                StartZoomIn();
            }
        }
        else if (fovZoomActive && !currentlyZooming){
            StopCoroutine("ZoomFOV");
            StartZoomOut();
        }

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * currentWalkSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);

        if (horizontalInput != new Vector2(0, 0))
        {
            // Animation & Running //
            UpdateRunning();

            if (OnSlope())
            {
                controller.Move(Vector3.down * controller.height / 2 * slopeForce * Time.deltaTime);
            }
        }
        else
        {
            ControllerReferences.playerAnim.SetInteger("Walking", 0);
        }

    }

    // MOUSE LOOK //
    void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = mouseLookInput;

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -75.0f, 75.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);

        playerCamera.transform.position += shake.GetPosition();
    }

    public void StartZoomIn()
    {
        StartCoroutine(ZoomFOV(Camera.main.fieldOfView, fovZoomAmount, true));
        fovZoomActive = true;
    }

    public void StartZoomOut()
    {
        StartCoroutine(ZoomFOV(Camera.main.fieldOfView, playerFov, false));
        fovZoomActive = false;
    }

    private IEnumerator ZoomFOV(float startFOV, float endFOV, bool zoomIn){
        currentlyZooming = true;
        if(zoomIn){
            fovZoomActive = true;
        }
        else{
            fovZoomActive = false;
        }

        float t = 0.0f;

        // continue the lerp for the time we have left
        while(t < fovZoomTime)
        {
            Camera.main.fieldOfView = Mathf.Lerp(startFOV, endFOV, t / fovZoomTime);
            t += Time.deltaTime;
            yield return null;
        }

        Camera.main.fieldOfView = endFOV;
        currentlyZooming = false;
    }
    #endregion



    // RUNNING //
    #region Running

    void UpdateRunning()
    {
        if (running == 1 && canRun)
        {
            ControllerReferences.playerAnim.SetInteger("Walking", 2);
            currentWalkSpeed = walkSpeed * runSpeedMultiplier;
            currentStamina.value -= Time.deltaTime * staminaDrainRate;
            if (currentStamina.value <= 0)
            {
                canRun = false;
            }
            else if (currentStamina.value == maxStamina.value)
            {
                canRun = true;
            }
        }
        else
        {
            ControllerReferences.playerAnim.SetInteger("Walking", 1);
            currentWalkSpeed = walkSpeed;
        }
    }

    void UpdateStamina()
    {
        if (running != 1 || horizontalInput == new Vector2(0, 0) || !canRun)
        {
            if (currentStamina.value < maxStamina.value)
            {
                currentStamina.value += Time.deltaTime * staminaRefillRate;
            }
            else
            {
                canRun = true;
            }
        }
    }
    #endregion


        
    


    // PHYSICS //
    #region Physics 
    // GRAVITY //
    void UpdateGravity()
    {
        if (controller.isGrounded)
        {
            velocityY = 0.0f;
        }

        velocityY += gravity * Time.deltaTime;
    }


    // SLOPE CHECK //
    private bool OnSlope()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2 * slopeForceRayLength))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }
    #endregion


    public void Interact(){
      	playerCamera.GetComponent<InteractionRaycast>().TriggerInteract();
	}

    
    // INPUT //
    #region Input

    public void ReceiveInput(Vector2 _horizontalInput, Vector2 _mouseLookInput, float _running, float _aiming)
    {
        horizontalInput = _horizontalInput;
        mouseLookInput = _mouseLookInput;
        running = _running;
        aiming = _aiming;
    }

    public void ControllerEnabled()
    {
        controller.enabled = true;
    } 
    
    public void ControllerDisabled()
    {
        controller.enabled = false;
    }

    public void ResetInput(){
        horizontalInput = new Vector2(0f,0f);
        mouseLookInput = new Vector2(0f,0f);
        running = 0;
        aiming = 0;

        cameraPitch = 0f;
    }
    #endregion
}
