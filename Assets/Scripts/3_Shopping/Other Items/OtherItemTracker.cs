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
        // Create a List of ItemSOs from the ShoppingList ((This should really be moved to the ShoppingListInventory Script))
        var shoppingList = GameObject.FindWithTag("Shopping List").GetComponent<ShoppingListInventory>().CheckShoppingList();
        List<ItemSO> shoppingListItemSOs = new List<ItemSO>();
            for(int i = 0; i < shoppingList.Count; i++){
                shoppingListItemSOs.Add(shoppingList[i].itemSO);
            }

        // Create Inventory List and Remove ShoppingListItems
        List<ItemSO> inventory = ControllerReferences.playerInventory.CheckInventory().Except(shoppingListItemSOs).ToList();

            for(int i = inventory.Count-1; i > -1; i--){
                if(inventory[i] == null){
                    // Clear out empty slots
                    Debug.Log($"Inventory Item #{i+1} Removed");
                    inventory.Remove(inventory[i]);
                }
                else{
                    // If the item is not Allowed, add to otherItems List as Unpaid
                    if(otherItems.Any(OtherItem => OtherItem.itemSO == inventory[i])){
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
