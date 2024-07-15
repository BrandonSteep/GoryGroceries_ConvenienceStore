using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningDoors : MonoBehaviour, IInteractable
{
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float lerpTime;
    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 endPoint;
    private bool open = false;
    
    void Start()
    {
        startPoint = this.transform.localPosition;
    }
    
    public void Interact(){
        Debug.Log($"Start Point = {startPoint} & End Point = {endPoint}");
        if(!open){
            open = true;
            StartCoroutine(LerpValue(this.transform.localPosition, endPoint, lerpTime));
        }
        else{
            open = false;
            StartCoroutine(LerpValue(this.transform.localPosition, startPoint, lerpTime));
        }
    }


    private IEnumerator LerpValue(Vector3 start, Vector3 end, float duration){
        float timeElapsed = 0f;

        while(timeElapsed < duration){
            float t = timeElapsed / duration;
            t = curve.Evaluate(t);

            var newPos = Vector3.Lerp(start, end, t);

            // Debug.Log($"{newPos}");
            transform.localPosition = newPos;

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = end;
    }
}
