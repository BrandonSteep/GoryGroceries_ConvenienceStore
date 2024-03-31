using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO item;
    [SerializeField] private float interpolationDuration = 0.75f;

    public void Interact()
    {
        if(ControllerReferences.playerInventory.AddToInventory(item)){
            GetComponent<Collider>().enabled = false;
            GetComponent<SmoothDampInterpolate>().BeginInterpolation(interpolationDuration);
            Destroy(this.gameObject, interpolationDuration);
        }
    }
}
