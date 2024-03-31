using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSwapper : MonoBehaviour
{
    [SerializeField] private Transform itemSlot;

    void Awake(){
        itemSlot = transform.GetChild(0);
    }

    public void RefreshEquipment(int index){
        RemoveCurrentItem();

        var newItem = GetCurrentSlot(index);
        // Debug.Log(newItem);

        if(newItem != null){
            InstantiateNewItem(newItem.heldItem);
        }

        ControllerReferences.playerInventory.inventoryUI.SetActiveUI();
    }

    public void EquipItemByIndex(int index){
        RemoveCurrentItem();
        
        if(GetCurrentSlot(index) != null){
            InstantiateNewItem(GetCurrentSlot(index).heldItem);
        }
    }

    public ItemSO GetCurrentSlot(int index){
        return ControllerReferences.playerInventory.GetItemAtIndex(index);
    }

    private void InstantiateNewItem(GameObject nextItem){
        Instantiate(nextItem, itemSlot);
    }

    private void RemoveCurrentItem(){
        if(itemSlot.childCount > 0){
            Destroy(itemSlot.GetChild(0).gameObject);
        }
    }
}
