using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutBag : MonoBehaviour, IInteractable
{
    public CheckoutInventory checkoutInventory;
    [SerializeField] private Transform[] bagSlots;
    [SerializeField] private float interpolationDuration = .5f;

    public void Interact(){
        if(!checkoutInventory.hasPaid){

            ItemSO currentItem = ControllerReferences.playerInventory.GetItemAtIndex(ControllerReferences.playerInventory.currentlyEquipped.GetCurrentIndex());

            if(!checkoutInventory.canScan && checkoutInventory.lastScannedItem == currentItem){
                AddItemToBag(currentItem.itemInBag);
                ControllerReferences.playerInventory.RemoveFromInventory(currentItem);

                checkoutInventory.CanScan();
            }
        }
        else{
            GetComponent<Collider>().enabled = false;
            GetComponent<SmoothDampInterpolate>().BeginInterpolation(interpolationDuration);
            Invoke("ResetCheckout", interpolationDuration);
            Destroy(this.gameObject, interpolationDuration);
        }
    }

    private void ResetCheckout(){
        checkoutInventory.ResetCheckout();
    }

    private void AddItemToBag(GameObject objToAdd){
        for(int i = 0; i<bagSlots.Length; i++){
            if(bagSlots[i].childCount > 0){
                continue;
            }
            else{
                Instantiate(objToAdd, bagSlots[i]);
                break;
            }
        }
    }
}
