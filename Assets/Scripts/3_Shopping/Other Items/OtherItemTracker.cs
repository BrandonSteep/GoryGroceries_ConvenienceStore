using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherItemTracker : MonoBehaviour
{
    private List<OtherItem> otherItems;
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
}
