using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTimer : MonoBehaviour
{
    [SerializeField] private float disableTime;

    void OnEnable(){
        Invoke("DisableGO", disableTime);
    }

    private void DisableGO(){
        this.gameObject.SetActive(false);
    }
}
