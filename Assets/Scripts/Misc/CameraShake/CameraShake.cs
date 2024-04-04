using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private bool shakeEnabled = true;

    private Vector3 shakePosition;

    [Header("Tiny Shake")]
    [SerializeField] private float tinyShakeDuration;
    [SerializeField] private float tinyShakeMagnitude;


    [Header("Small Shake")]
    [SerializeField] private float smallShakeDuration;
    [SerializeField] private float smallShakeMagnitude;


    [Header("Mid Shake")]
    [SerializeField] private float midShakeDuration;
    [SerializeField] private float midShakeMagnitude;


    [Header("Big Shake")]
    [SerializeField] private float bigShakeDuration;
    [SerializeField] private float bigShakeMagnitude;

    public Vector3 GetPosition(){
        return shakePosition;
    }

    public void TinyShake(){
        if(shakeEnabled){
            StartCoroutine(PerformShake(tinyShakeDuration, tinyShakeMagnitude));
        }
    }
    
    public void SmallShake(){
        if(shakeEnabled){
            StartCoroutine(PerformShake(smallShakeDuration, smallShakeMagnitude));
        }
    }

    public void MidShake(){
        if(shakeEnabled){
            StartCoroutine(PerformShake(midShakeDuration, midShakeMagnitude));
        }
    }

    public void BigShake(){
        if(shakeEnabled){
            StartCoroutine(PerformShake(bigShakeDuration, bigShakeMagnitude));
        }
    }

    private IEnumerator PerformShake(float duration, float magnitude){
        // Debug.Log("Shaking Screen");

        float time = 0.0f;

        while (time < duration){
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            shakePosition = new Vector3(x, y, 0f);

            time += Time.deltaTime;

            yield return null;
        }

        shakePosition = Vector3.zero;
    }
}
