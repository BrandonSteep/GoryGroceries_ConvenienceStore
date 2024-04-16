using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class OtherItemTracker : MonoBehaviour
{
    private List<OtherItem> otherItems = new List<OtherItem>();
    public List<OtherItem> GetOtherItems(){
        return otherItems;
    }

    public void AddNewPaidItem(ItemSO itemToAdd){
        otherItems.Add(new OtherItem(itemToAdd, OtherItemStatus.Paid));
    }

    public void AddNewUnpaidItem(ItemSO itemToAdd){
        otherItems.Add(new OtherItem(itemToAdd, OtherItemStatus.Unpaid));
    }

    public void AddAllowedItem(ItemSO itemToAdd){
        otherItems.Add(new OtherItem(itemToAdd, OtherItemStatus.Allowed));
    }

    public void AddAllHeldItems(){
        // Create a List of ItemSOs from the ShoppingList
        // THIS should really be moved to the ShoppingListInventory Script
        var shoppingList = GameObject.FindWithTag("Shopping List").GetComponent<ShoppingListInventory>().CheckShoppingList();
        List<ItemSO> shoppingListItemSOs = new List<ItemSO>();
            for(int i = 0; i < shoppingList.Count; i++){
                shoppingListItemSOs.Add(shoppingList[i].itemSO);
            }

        // Create 'inventory' List from PlayerInventory and Remove all items from shoppingListItemSO
        List<ItemSO> inventory = ControllerReferences.playerInventory.CheckInventory().Except(shoppingListItemSOs).ToList();
        // Create 'otherItemsOnShoppingList' List from PlayerInventory and Remove all items NOT found in shoppingListItemSO
        List<ItemSO> otherItemsOnShoppingList = ControllerReferences.playerInventory.CheckInventory();
        otherItemsOnShoppingList.RemoveAll( itemSO => !shoppingListItemSOs.Contains(itemSO));

        // Loop through the 'otherItemsOnShoppingList' List to 
        for(int i = otherItemsOnShoppingList.Count-1; i > -1; i--){

            // Get ShoppingItem with the same ItemSO
            var shoppingItem = shoppingList.First(ShoppingItem => ShoppingItem.itemSO == otherItemsOnShoppingList[i]);
            // Check whether the Item is NOT PAID
            if(shoppingItem.status != ShoppingItemStatus.Paid){
                // IF NOT, Remove ONE version of that item from 'otherItemsOnShoppingList' (as one of them will be included in the MISSION total)
                Debug.Log("Removing Item, as it will be counted as STOLEN in Mission Total");
                otherItemsOnShoppingList.Remove(shoppingItem.itemSO);
            }
            // If one is found, do nothing - all of this item held in the inventory should be considered stolen
        }
        // Add the remaining 'otherItemsOnShoppingList' List onto the 'inventory' List, ensuring that all items that should be considered stolen are 
        inventory.AddRange(otherItemsOnShoppingList);

        for(int i = inventory.Count-1; i > -1; i--){
            if(inventory[i] == null){
                // Clear out empty slots
                Debug.Log($"Inventory Item #{i+1} Removed");
                inventory.Remove(inventory[i]);
                }
            else{
                // If the item is not Allowed, add to otherItems List as Unpaid
                if(otherItems.Any(OtherItem => OtherItem.itemSO == inventory[i])){
                    Debug.Log($"Item {inventory[i].name} is Allowed");
                    continue;
                }
                else{
                    Debug.Log($"Adding {inventory[i].name} to the Other Items List as UNPAID");
                    otherItems.Add(new OtherItem(inventory[i], OtherItemStatus.Unpaid));
                    }
                }
            }
        if(otherItems.Count == 0){
            Debug.Log("No Other Items");
        }

        SetGameManagerOtherItemList();
    }

    public void SetGameManagerOtherItemList(){
        GameManager.otherItems.Clear();
        for(int i = 0; i < otherItems.Count; i++){
            GameManager.otherItems.Add(otherItems[i]);
        }
    }
}
