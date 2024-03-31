using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ShoppingItemUIElement : MonoBehaviour
{
    [HideInInspector]
    public ShoppingItem thisItem;
    [HideInInspector]
    public ShoppingListInventory shoppingList;

    public void SetName(){
        GetComponent<TextMeshProUGUI>().text = thisItem.itemSO.itemName;
    }

    public void SetItemStatus(){
        if(shoppingList.selectedItems.Find(i => i.itemSO == thisItem.itemSO).status != ShoppingItemStatus.Unchecked){
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
        else{
            this.transform.GetChild(0).gameObject.SetActive(false);
            // Debug.Log("Item Set as Unchecked");
        }
    }

    void OnEnable(){
        SetItemStatus();
    }
}
