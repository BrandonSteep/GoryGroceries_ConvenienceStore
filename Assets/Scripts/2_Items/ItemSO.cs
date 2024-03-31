using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public float price;
    public GameObject pickup;
    public GameObject itemInBag;
    public GameObject heldItem;
    public Sprite image;
}
