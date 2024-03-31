using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyValueUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    void Awake(){
        text = GetComponent<TextMeshProUGUI>();
    }

    public void SetPrice(float totalAmount){

        text.text = totalAmount.ToString("c2");
    }
}
