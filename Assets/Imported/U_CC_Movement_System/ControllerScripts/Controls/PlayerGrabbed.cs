using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabbed : MonoBehaviour
{

    [Header("Interpolation Variables")]
    [SerializeField] private AnimationCurve interpolationCurve;
    [SerializeField] private float interpolationDuration = 0.75f;
    [Header("Look At Variables")]
    [SerializeField] private float lookAtSpeed = 2f;
    [SerializeField] private LookAt lookAt;
    [SerializeField] private LookAt cameraLookAt;

    public void GrabbedByZombie(Collider other){
        StateMachineController grabbingEnemy = other.transform.parent.GetComponent<StateMachineController>();
        grabbingEnemy.SetAnimTrigger("Bite");

        ControllerReferences.playerController.ControllerDisabled();
        // ControllerReferences.playerController.enabled = false;

        ControllerReferences.playerAnim.SetInteger("Walking", 0);
        

        Debug.Log($"player position moving to {other.transform.position}");
        // Move Player Into Position
        StartCoroutine(LerpPosition(new Vector3(other.transform.position.x, this.transform.position.y, other.transform.position.z)));


        // Make Player Look At Zombie
        if(lookAt == null){
            lookAt = GetComponent<LookAt>();
        }
        Debug.Log("Looking At Zombie");
        Vector3 horizontalLookAtPosition = new Vector3(grabbingEnemy.transform.position.x, transform.position.y, grabbingEnemy.transform.position.z);
        lookAt.StartLookAt(this.transform, lookAtSpeed, horizontalLookAtPosition);
        cameraLookAt.StartLookAt(cameraLookAt.transform, lookAtSpeed, grabbingEnemy.CheckFocalPoint().position);

        // Move Enemy to Player
        grabbingEnemy.MoveToPlayer();
    }
    public void EndGrab(){
        // ControllerReferences.playerController.enabled = true;
        ControllerReferences.playerController.ControllerEnabled();
        ControllerReferences.playerController.ResetInput();
        ControllerReferences.playerKnockback.AddImpact(-transform.forward, 15f);
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
        Debug.Log($"Setting Player Position to {endPosition}");
        transform.position = endPosition;
    }
}
