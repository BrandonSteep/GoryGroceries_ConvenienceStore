using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiptManager : MonoBehaviour
{
    [SerializeField] private RectTransform layoutGroup;
    [SerializeField] private GameObject receiptItem;
    [SerializeField] private GameObject divider;
    [SerializeField] private GameObject comeBackSoon;

    public void OnEnable(){

        float totalPrice = 0f;
        
        // Debug.Log($"'{gameManager.currentItems}' Contents = {gameManager.currentItems.Count}");

        
        for(int i = 0; i<GameManager.currentItems.Count; i++){
        
            Debug.Log($"Number {i+1} of List = {GameManager.currentItems[i].itemSO.name}");
        

            var newItem = Instantiate(receiptItem, layoutGroup.transform).GetComponent<CheckoutMenuItem>();

            newItem.SetDetails(GameManager.currentItems[i].itemSO);
            if(GameManager.currentItems[i].status == ShoppingItemStatus.Paid){
                totalPrice += GameManager.currentItems[i].itemSO.price;
            }
            else if(GameManager.currentItems[i].status == ShoppingItemStatus.Unchecked){
                newItem.SetPriceManually("FAILED");
            }
            else if(GameManager.currentItems[i].status == ShoppingItemStatus.Checked){
                newItem.SetPriceManually("STOLEN");
            }
        }

        Instantiate(divider, layoutGroup);

        var totals = Instantiate(receiptItem, layoutGroup).GetComponent<CheckoutMenuItem>();
        totals.SetNameManually("Totals");
        totals.SetPriceManually(totalPrice);

        Instantiate(divider, layoutGroup);
        Instantiate(comeBackSoon, layoutGroup);
    }
}
