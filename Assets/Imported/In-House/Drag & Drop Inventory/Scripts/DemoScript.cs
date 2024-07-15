using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    public void PickupItem(int id){
        bool result = inventoryManager.AddItem(itemsToPickup[id]);
        if(!result){
            Debug.Log($" ITEM [{itemsToPickup[id].name}] NOT PICKED UP");
        }
        else{
            Debug.Log($"Item [{itemsToPickup[id].name}] Successfully Picked Up");
        }
    }

    public void GetSelectedItem(){
        Item receivedItem = inventoryManager.GetSelectedItem(false);
        if(receivedItem != null){
            Debug.Log($"Received Item [{receivedItem.name}]");
        }
        else{
            Debug.Log("No item received");
        }
    }

        public void UseSelectedItem(){
        Item receivedItem = inventoryManager.GetSelectedItem(true);
        if(receivedItem != null){
            Debug.Log($"Used Item [{receivedItem.name}]");
        }
        else{
            Debug.Log("No item to use");
        }
    }
}
