using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : MonoBehaviour
{
    [SerializeField] private PlayerController controls;
    [SerializeField] private Animator anim;
    [SerializeField] private InteractionRaycast interact;
    [SerializeField] private float translationSpeed = 5f;
    private Vector3 resetPos;
    private Quaternion resetRot;
    private float sinTime;

    void Start(){
        GameObject player = GameObject.FindWithTag("Player");
        controls = player.GetComponent<PlayerController>();
        anim = player.GetComponent<Animator>();

        interact = GetComponent<InteractionRaycast>();
    }

    public void ResetPlayer(){

        // Change the Cursor's Status
        ChangeCursorStatus(false);

        // Move the Camera
        StartCoroutine(SmoothTranslation(resetPos, resetRot, false));
    }

    public void MovePlayerToPosition(Transform posToMove, bool freeCursor = false){
        // Save Position & Rotation
        resetPos = this.transform.position;
        resetRot = this.transform.rotation;

        // Lock the Player's Movement
        TogglePlayerLock();

        // Move the Camera
        StartCoroutine(SmoothTranslation(posToMove.position, posToMove.rotation, true));

        // Change the Cursor's Status
        ChangeCursorStatus(freeCursor);
    }

    private void TogglePlayerLock(){
        controls.enabled = !controls.enabled;
        anim.enabled = !anim.enabled;

        if(interact.enabled){
            interact.interactUI.enabled = false;
            interact.enabled = false;
        }
        else{
            interact.enabled = true;
            interact.interactUI.enabled = true;
        }
    }

    private void ChangeCursorStatus(bool freeCursor){
        if(freeCursor){
            Cursor.lockState = CursorLockMode.Confined;
        }
        else{
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    IEnumerator SmoothTranslation(Vector3 targetPos, Quaternion targetRot, bool lockControls) {
        // Debug.Log($"Moving Player to Position: {posToMove.position} and Rotation: {posToMove.rotation.eulerAngles}");
        bool translating = true;
        sinTime = 0f;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f && Quaternion.Angle(transform.rotation, targetRot) > 0.1f && translating){
            sinTime += Time.deltaTime * translationSpeed;
            sinTime = Mathf.Clamp(sinTime, 0, Mathf.PI);
            float t = Evaluate(sinTime);

            this.transform.position = Vector3.Lerp (this.transform.position, targetPos, Time.deltaTime * translationSpeed);
            this.transform.rotation = Quaternion.Slerp (this.transform.rotation, targetRot, Time.deltaTime * translationSpeed);
            yield return null;
        }

        targetPos = Vector3.zero;
        targetRot = Quaternion.identity;
        translating = false;

        if(!lockControls){
            TogglePlayerLock();
        }

        yield return null;
    }

    private float Evaluate(float x){
        return 0.5f * Mathf.Sin(x - Mathf.PI / 2f) + 0.5f;
    }
}
