using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardReader : MonoBehaviour, IInteractable
{
    [SerializeField] private CheckoutInventory checkoutInventory;
    
    public void Interact(){
        if(ControllerReferences.playerWallet.PayForShopping(checkoutInventory.CheckoutTotal()) && checkoutInventory.canScan){
            checkoutInventory.CheckoutSuccessful();
        }
        else{
            Debug.Log($"Checkout Failed");
            checkoutInventory.CheckoutFailed();
        }
    }
}
