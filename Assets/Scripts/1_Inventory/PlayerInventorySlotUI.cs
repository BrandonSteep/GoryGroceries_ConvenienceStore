using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image spriteImage;
    private Color visible;
    private Color invisible;

    void Start(){
        visible = Color.white;
        visible.a = 1;

        invisible = Color.white;
        invisible.a = 0;
    }

    public void AddItem(ItemSO item){
        // Debug.Log($"{this.gameObject} contains {item.name}");
        spriteImage.sprite = item.image;

        spriteImage.color = visible;
    }    

    public void ClearSlot(int i){
        // Debug.Log($"Clearing Slot {i}");
        spriteImage.color = invisible;
    }
}
