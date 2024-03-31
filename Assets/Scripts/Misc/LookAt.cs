using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    private Coroutine LookCoroutine;

    public void StartLookAt(float lookAtSpeed){
        if(LookCoroutine != null){
            StopCoroutine(LookCoroutine);
        }
        LookCoroutine = StartCoroutine(LookAtPosition(lookAtSpeed));
    }
    
    private IEnumerator LookAtPosition(float lookAtSpeed){
        
        Vector3 lookPosition = new Vector3(ControllerReferences.player.transform.position.x, transform.position.y, ControllerReferences.player.transform.position.z);

        Quaternion lookRotation = Quaternion.LookRotation(lookPosition - transform.position);

        float time = 0f;

        while(time < 1){
            Quaternion newRotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            transform.rotation = newRotation;

            time += Time.deltaTime * lookAtSpeed;
            yield return null;
        }
    }
}
