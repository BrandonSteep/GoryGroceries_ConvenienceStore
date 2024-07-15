using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;
    
    private void Awake(){
        Deselect();
    }

    public void Select(){
        image.color = selectedColor;
    }

    public void Deselect(){
        image.color = notSelectedColor;
    }

    // Drag and Drop
    public void OnDrop(PointerEventData eventData){
        InventoryItem heldItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        if(transform.childCount != 0){
            InventoryItem slotItem = transform.GetChild(0).GetComponent<InventoryItem>();
            SlotFilled(heldItem, slotItem);
        }
        else{
            heldItem.parentAfterDrag = transform;
        }
    }

    // Slot Filled Behaviours
    #region Slot Filled
    private void SlotFilled(InventoryItem heldItem, InventoryItem slotItem){
        if(heldItem.item == slotItem.item && slotItem.item.stackable && slotItem.count < slotItem.item.maxStackedItems){
            // Debug.Log($"Slot Item's quantity is lower than {slotItem.item.maxStackedItems} - combining stacks now");
            CombineStacks(heldItem, slotItem);
        }
        else{
            SwapItems(heldItem, slotItem);
        }
    }

    private void CombineStacks(InventoryItem heldItem, InventoryItem slotItem){
  // TRIGGER MATHF.MIN CALCULATION FROM HERE

        int transferAmount = heldItem.inventory.GetTransferAmount(heldItem.count, slotItem);

    //     int spaceInStack = slotItem.item.maxStackedItems - slotItem.count;
    //     int transferAmount = Mathf.Min(heldItem.count, spaceInStack);

        slotItem.count += transferAmount;
        heldItem.count -= transferAmount;

        slotItem.RefreshCount();

        if (heldItem.count == 0) {
            Destroy(heldItem.gameObject);
        }
        else{
            heldItem.RefreshCount();
            heldItem.transform.SetParent(heldItem.parentAfterDrag);
        }
    }

    private void SwapItems(InventoryItem heldItem, InventoryItem slotItem){
        // Debug.Log($"Swapping Items");
        if(heldItem.split == false){
            slotItem.transform.SetParent(heldItem.parentAfterDrag);
            heldItem.parentAfterDrag = transform;
        }
        else{
            InventoryItem previousItem = heldItem.parentAfterDrag.GetComponentInChildren<InventoryItem>();
            
            previousItem.split = false;
            previousItem.count += heldItem.count;
            previousItem.RefreshCount();
            Destroy(heldItem.gameObject);
        }
    }
    #endregion
}
