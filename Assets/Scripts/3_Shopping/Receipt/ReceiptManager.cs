using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReceiptManager : MonoBehaviour
{
    [SerializeField] private RectTransform layoutGroup;
    [SerializeField] private GameObject receiptItem;
    [SerializeField] private GameObject divider;
    [SerializeField] private GameObject comeBackSoon;
    private float totalPrice = 0f;

    public void OnEnable(){

        AddShoppingListItems();
        AddOtherItems();

        Instantiate(divider, layoutGroup);

        var totals = Instantiate(receiptItem, layoutGroup).GetComponent<CheckoutMenuItem>();
        totals.SetNameManually("Totals");
        totals.SetPriceManually(totalPrice);

        Instantiate(divider, layoutGroup);
        Instantiate(comeBackSoon, layoutGroup);
    }


    private void AddShoppingListItems(){
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
    }

    private void AddOtherItems(){

        if(GameManager.otherItems.Count > 0){
            var othersSubTotal = 0f;
            int itemsStolen = 0;

            for(int i = 0; i < GameManager.otherItems.Count; i++){
                Debug.Log($"Other item {GameManager.otherItems[i].itemSO.name} added to Receipt Total");

                if(GameManager.otherItems[i].status == OtherItemStatus.Unpaid){
                    itemsStolen++;
                }
                else if (GameManager.otherItems[i].status == OtherItemStatus.Paid){
                    othersSubTotal += GameManager.otherItems[i].itemSO.price;
                }
            }

            if(itemsStolen < GameManager.otherItems.Count){
                var otherReceiptItem = Instantiate(receiptItem, layoutGroup.transform).GetComponent<CheckoutMenuItem>();
                otherReceiptItem.SetNameManually("OTHERS");
                otherReceiptItem.SetPriceManually(othersSubTotal);
                totalPrice += othersSubTotal;
            }

            if(itemsStolen > 0){
                var otherStolenReceiptItem = Instantiate(receiptItem, layoutGroup.transform).GetComponent<CheckoutMenuItem>();
                otherStolenReceiptItem.SetNameManually("OTHERS");
                otherStolenReceiptItem.SetPriceManually("STOLEN");
            }
        }
        else{
            Debug.Log("No Other Items Found");
        }
    }
}
