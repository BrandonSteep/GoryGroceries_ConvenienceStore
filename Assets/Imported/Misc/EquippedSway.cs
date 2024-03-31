using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedSway : MonoBehaviour
{
    // INPUT //
    private Vector2 mouseLookInput;
    private Vector3 initialPosition;


    // Parameters //
    public float amount = -0.025f;
    public float maxAmount = 0.1f;
    public float smoothAmount = 4f;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void FixedUpdate()
    {
        mouseLookInput = ControllerReferences.playerController.mouseLookInput * amount;

        //Clamp the Sway//
        mouseLookInput.x = Mathf.Clamp(mouseLookInput.x, -maxAmount, maxAmount);
        mouseLookInput.y = Mathf.Clamp(mouseLookInput.y, -maxAmount, maxAmount);

        //Move the Object//
        Vector3 finalPosition = new Vector3(mouseLookInput.x, mouseLookInput.y * 0.5f, 0);
        transform.localPosition = Vector3.Slerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);
    }
}
