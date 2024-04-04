using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    [Header ("Money")]
    [SerializeField] private ScriptableVariable playerMoney;
    [SerializeField] private MoneyValueUI playerMoneyUI;
    
    void Start(){
        RefreshPlayerMoneyUI();
    }

    public void RefreshPlayerMoneyUI(){
        // Debug.Log($"Player Money = {playerMoney.value.ToString("c2")}");
        playerMoneyUI.SetPrice(playerMoney.value);
    }

    public bool PayForShopping(float shoppingTotal){
        if(playerMoney.value >= shoppingTotal && shoppingTotal > 0f){
            playerMoney.value -= shoppingTotal;
            RefreshPlayerMoneyUI();
            return true;
        }
        else{
            return false;
        }
    }

    public void AddMoney(float moneyIn){
        playerMoney.value += moneyIn;
        RefreshPlayerMoneyUI();
    }
}
