using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionRaycast : MonoBehaviour
{
    [SerializeField] private float interactionDistance;
    [SerializeField] private LayerMask targetableLayers;

    public RawImage interactUI;

    private bool interactTrigger;

    void Start(){
        interactUI = GameObject.FindWithTag("InteractUI").GetComponent<RawImage>();
        if(interactUI != null){
            // Debug.Log("Interact UI Set Successfully");
        }
        else{
            Debug.LogWarning("*Interact UI NOT SET*");
        }
    }

    void Update()
    {
        if(!GetInteractable()){
            ResetUI();
        }
    }

    bool GetInteractable(){
        RaycastHit hit;
        if(Physics.Raycast(this.transform.position, transform.forward, out hit, interactionDistance, targetableLayers)){
            if(hit.collider.tag == "Interactable"){
                interactUI.color = new Color32(255, 255, 255, 255);
                
                if(interactTrigger){
                    hit.collider.GetComponent<IInteractable>().Interact();
                    DisableInteract();
                }

                return true;
            }
            else{
                return false;
            }
        }
        return false;
    }

    void ResetUI(){
        interactUI.color = new Color32(255, 255, 255, 0);
    }

    public void TriggerInteract(){
        interactTrigger = true;
        // Debug.Log("Interaction Triggered");
        Invoke("DisableInteract", 0.1f);
    }

    void DisableInteract(){
        if(interactTrigger){
            interactTrigger = false;
            // Debug.Log("Interaction Ended");
        }
    }
}
