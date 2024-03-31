using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothDampInterpolate : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;

    public void BeginInterpolation(float duration){
        StartCoroutine(LerpValue(transform.position, duration));
    }

    private IEnumerator LerpValue(Vector3 start, float duration){
        float timeElapsed = 0f;

        while(timeElapsed < duration){
            float t = timeElapsed / duration;
            t = curve.Evaluate(t);

            var newPos = Vector3.Lerp(start, ControllerReferences.playerInventory.flyTo.position, t);

            // Debug.Log($"{newPos}");
            transform.position = newPos;

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = ControllerReferences.player.transform.position;
    }
}
