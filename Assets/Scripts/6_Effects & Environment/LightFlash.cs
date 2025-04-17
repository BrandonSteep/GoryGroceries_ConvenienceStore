using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlash : MonoBehaviour
{
    private Light lightToFlash;
    
    [SerializeField] private float baseIntensity;
    [SerializeField] private float flashIntensity = 3;
    [SerializeField] private float flashLength;

    void OnEnable(){
        lightToFlash = GetComponent<Light>();
        baseIntensity = lightToFlash.intensity;
    }

    public void Flash(){
        Debug.Log("Flashing Light Now");
        lightToFlash.intensity = flashIntensity;
        Invoke("EndFlash", flashLength);
    }

    private void EndFlash(){
        lightToFlash.intensity = baseIntensity;
    }
}
