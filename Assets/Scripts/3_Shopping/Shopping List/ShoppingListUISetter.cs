using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShoppingListUISetter : MonoBehaviour
{
    [SerializeField] private GameObject itemTextTemplate;
    [SerializeField] private RectTransform listContainer;

    public void AddListItems(List<ShoppingItem> selectedItems){
        for(int i = 0; i<selectedItems.Count; i++){
            ShoppingItemUIElement itemText = Instantiate(itemTextTemplate, listContainer).GetComponent<ShoppingItemUIElement>();
            itemText.shoppingList = GetComponent<ShoppingListInventory>();
            itemText.thisItem = selectedItems[i];
            itemText.SetName();
        }
    }
}
