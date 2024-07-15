using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScreen : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform camTransform;
    [SerializeField] private GameObject screensaver;
    [SerializeField] private GameObject cursor;

    public void Interact(){
        Camera.main.transform.GetComponent<CameraLock>().MovePlayerToPosition(camTransform, true);
        ToggleScreen();
    }

    public void ToggleScreen(){
        screensaver.SetActive(!screensaver.activeInHierarchy);
        cursor.SetActive(!cursor.activeInHierarchy);
    }
}
