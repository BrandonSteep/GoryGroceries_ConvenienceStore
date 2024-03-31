using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutScanner : MonoBehaviour, IInteractable
{
    [SerializeField] private CheckoutInventory checkoutInventory;
    public void Interact(){
        checkoutInventory.ScanItem();
    }
}
