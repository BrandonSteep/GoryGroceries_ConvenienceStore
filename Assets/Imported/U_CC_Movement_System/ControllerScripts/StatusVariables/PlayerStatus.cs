using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status, IStatus
{
    [Tooltip("This value is set in Seconds")][SerializeField] private float iFrames = 1f;

    public void TakeDamage()
    {
        ControllerReferences.playerAnim.SetTrigger("TakeDamage");
        currentHp.value -= 25f;
        canTakeDamage = false;
    }

    public void ResetDamage()
    {
        canTakeDamage = true;
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Grab Trigger" && canTakeDamage){
            canTakeDamage = false;
            Debug.Log("Player Grabbed!");
            GrabbedByZombie(other);
        }
    }

#region Zombie Grab Logic

    [Header("Zombie Grab Variables")]
    // Smooth Damp Interpolation for Zombie Grab Movement
    [SerializeField] private AnimationCurve interpolationCurve;
    [SerializeField] private float interpolationDuration = 0.75f;
    [SerializeField] private LookAt lookAt;
    [SerializeField] private LookAt cameraLookAt;
    [SerializeField] private float lookAtSpeed = 2f;

    public void GrabbedByZombie(Collider other){
        StateMachineController grabbingEnemy = other.transform.parent.GetComponent<StateMachineController>();
        grabbingEnemy.SetAnimTrigger("Bite");

        ControllerReferences.playerController.ControllerDisabled();
        // ControllerReferences.playerController.enabled = false;

        ControllerReferences.playerAnim.SetInteger("Walking", 0);
        

        // Move Player Into Position
        StartCoroutine(LerpPosition(new Vector3(other.transform.position.x, other.transform.position.y -.1f, other.transform.position.z)));


        // Make Player Look At Zombie
        if(lookAt == null){
            lookAt = GetComponent<LookAt>();
        }
        Debug.Log("Looking At Zombie");
        Vector3 horizontalLookAtPosition = new Vector3(grabbingEnemy.transform.position.x, transform.position.y, grabbingEnemy.transform.position.z);
        lookAt.StartLookAt(this.transform, lookAtSpeed, horizontalLookAtPosition);

        cameraLookAt.StartLookAt(cameraLookAt.transform, lookAtSpeed, grabbingEnemy.CheckFocalPoint().position);
    }
    public void EndGrab(){
        // ControllerReferences.playerController.enabled = true;
        ControllerReferences.playerController.ControllerEnabled();
        ControllerReferences.playerController.ResetInput();
        ControllerReferences.playerKnockback.AddImpact(-transform.forward, 15f);
        Invoke("ResetDamage", iFrames);
    }

    
    private IEnumerator LerpPosition(Vector3 endPosition){
        float timeElapsed = 0f;

        while(timeElapsed < interpolationDuration){
            float t = timeElapsed / interpolationDuration;
            t = interpolationCurve.Evaluate(t);

            var newPos = Vector3.Lerp(transform.position, endPosition, t);

            // Debug.Log($"{newPos}");
            transform.position = newPos;

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = endPosition;
    }
#endregion
    
    public void Die()
    {

    }
}
