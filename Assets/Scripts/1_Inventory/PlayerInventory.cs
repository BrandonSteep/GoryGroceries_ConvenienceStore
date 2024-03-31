using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private List<ItemSO> heldItems;
    [SerializeField] private int inventoryLimit;
    [SerializeField] private ItemSO mostRecentlyCollected;

    public Transform flyTo;

    [SerializeField] private ShoppingListInventory shoppingList;
    [SerializeField] private GameEvent newItemEvent;

    [Header("UI")]
    public PlayerInventoryUI inventoryUI;

    [Header("Currently Equipped")]
    public CurrentlyEquipped currentlyEquipped;

    void Start(){
        heldItems = new List<ItemSO>(new ItemSO[inventoryLimit]);
        shoppingList = GameObject.FindWithTag("Shopping List").GetComponent<ShoppingListInventory>();
    }
    
    public bool AddToInventory(ItemSO itemToAdd){

        if(heldItems[currentlyEquipped.GetCurrentIndex()] == null){
            heldItems[currentlyEquipped.GetCurrentIndex()] = itemToAdd;

            if(shoppingList.CheckItemStatus(itemToAdd) != ShoppingItemStatus.Paid){
                shoppingList.UpdateShoppingListItemStatus(itemToAdd, ShoppingItemStatus.Checked);
            }

            mostRecentlyCollected = itemToAdd;

            newItemEvent.Raise();
            currentlyEquipped.slotSwapper.RefreshEquipment(currentlyEquipped.GetCurrentIndex());

            return true;
        }
        else return false;
    }

    public void RemoveFromInventory(ItemSO itemToRemove){
        heldItems[currentlyEquipped.GetCurrentIndex()] = null;
        currentlyEquipped.slotSwapper.RefreshEquipment(currentlyEquipped.GetCurrentIndex());
        inventoryUI.RefreshUI();
    }

    public List<ItemSO> CheckInventory(){
        return heldItems;
    }

    public ItemSO GetMostRecentlyCollected(){
        return mostRecentlyCollected;
    }

    public ItemSO GetItemAtIndex(int i){
        if(i >= 0 && i < heldItems.Count){
            // Debug.Log($"Returning {heldItems[i]} at index {i}");
            return heldItems[i];
        }
        else {
            // Debug.Log($"Index {i} doesn't contain an item");
            return null;
        }
    }

    public ItemSO GetLastInventoryItem(){
        return heldItems[heldItems.Count-1];
    }

    public int GetFirstEmptySlot(){
        var slotIndex = heldItems.FindIndex(item => item == null);
        Debug.Log($"{slotIndex}");

        return slotIndex;
    }

    public int ReturnInventoryLimit(){
        return inventoryLimit;
    }

}
