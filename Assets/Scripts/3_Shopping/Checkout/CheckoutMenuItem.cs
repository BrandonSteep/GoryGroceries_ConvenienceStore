using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckoutMenuItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTextBox;
    [SerializeField] private TextMeshProUGUI priceTextBox;
    [SerializeField] private bool showCurrency = true;

    public void SetDetails(ItemSO itemSO){
        nameTextBox.text = itemSO.itemName;
        if(showCurrency){
            priceTextBox.text = itemSO.price.ToString("c2");
        }
        else{
            priceTextBox.text = itemSO.price.ToString("n2");
        }
    }

    public void SetNameManually(string name){
        nameTextBox.text = name;
    }

    public void SetPriceManually(float price){
        priceTextBox.text = $"{price.ToString("n2")}";
    }

        public void SetPriceManually(string price){
        priceTextBox.text = $"{price}";
    }
}
