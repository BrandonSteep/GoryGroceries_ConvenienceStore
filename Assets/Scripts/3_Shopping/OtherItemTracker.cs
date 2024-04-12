using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherItemTracker : MonoBehaviour
{
    private List<ShoppingItem> otherItems;
    public List<ShoppingItem> GetOtherItems(){
        return otherItems;
    }

    public void AddNewPaidItem(ItemSO itemToAdd){
        otherItems.Add(new ShoppingItem(itemToAdd, ShoppingItemStatus.Paid));
    }

    public void AddNewUsedItem(ItemSO itemToAdd){
        otherItems.Add(new ShoppingItem(itemToAdd, ShoppingItemStatus.Checked));
    }
}
