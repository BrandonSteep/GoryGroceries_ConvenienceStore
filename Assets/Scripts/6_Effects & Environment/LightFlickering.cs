using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickering : MonoBehaviour
{
    private Light light;
    [SerializeField] private float minLightSpeed = 0.025f;
    [SerializeField] private float maxLightSpeed = 0.5f;

    
    [SerializeField] private float minDarkSpeed = 0.5f;
    [SerializeField] private float maxDarkSpeed = 1.5f;

    void OnEnable(){
        light = GetComponent<Light>();
        Invoke("DisableLight", Random.Range(minDarkSpeed, maxDarkSpeed));
    }

    private void EnableLight(){
        light.enabled = true;
        Invoke("DisableLight", Random.Range(minLightSpeed, maxLightSpeed));
    }
    private void DisableLight(){
        light.enabled = false;
        Invoke("EnableLight", Random.Range(minDarkSpeed, maxDarkSpeed));
    }
}
