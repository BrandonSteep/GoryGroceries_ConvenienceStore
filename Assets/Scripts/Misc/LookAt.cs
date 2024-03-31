using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    private Coroutine LookCoroutine;

    public void StartLookAt(Transform objectToLook, float lookAtSpeed, Vector3 positionToLookAt){
        if(LookCoroutine != null){
            StopCoroutine(LookCoroutine);
        }
        LookCoroutine = StartCoroutine(LookAtPosition(objectToLook, lookAtSpeed, positionToLookAt));
    }
    
    private IEnumerator LookAtPosition(Transform objectToLook, float lookAtSpeed, Vector3 positionToLookAt){
        
        Quaternion lookRotation = Quaternion.LookRotation(positionToLookAt - objectToLook.position);

        float time = 0f;

        while(time < 1){
            Quaternion newRotation = Quaternion.Slerp(objectToLook.rotation, lookRotation, time);

            objectToLook.rotation = newRotation;

            time += Time.deltaTime * lookAtSpeed;
            yield return null;
        }
    }
}
