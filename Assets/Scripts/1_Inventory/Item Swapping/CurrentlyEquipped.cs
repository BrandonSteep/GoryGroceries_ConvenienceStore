using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentlyEquipped : MonoBehaviour
{
    public SlotSwapper slotSwapper;

    public ItemSO currentlyEquippedSO;
    private SwappableStatus swapStatus;

    [SerializeField] private int equippedIndex = 0;

    private GameObject nextItem;

    void Awake(){
        swapStatus = GetComponent<SwappableStatus>();
    }

    public int GetCurrentIndex(){
        return equippedIndex;
    }

#region Equip Input
    public void EquipNextSlot(){
        equippedIndex++;
        if(equippedIndex > ControllerReferences.playerInventory.ReturnInventoryLimit()-1){
            equippedIndex = 0;
        }
        ControllerReferences.playerInventory.inventoryUI.SetActiveUI();
        slotSwapper.EquipItemByIndex(equippedIndex);
    }
    public void EquipPreviousSlot(){
        equippedIndex--;
        if(equippedIndex < 0){
            equippedIndex = ControllerReferences.playerInventory.ReturnInventoryLimit()-1;
        }
        ControllerReferences.playerInventory.inventoryUI.SetActiveUI();
        slotSwapper.EquipItemByIndex(equippedIndex);
    }

    public void SelectSlot(int index){
        if(equippedIndex != index){
            equippedIndex = index;
            ControllerReferences.playerInventory.inventoryUI.SetActiveUI();
            slotSwapper.EquipItemByIndex(equippedIndex);
        }
    }
#endregion
}
