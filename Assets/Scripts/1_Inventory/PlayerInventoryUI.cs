using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUI : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;

    [Header("Inventory UI")]
    [SerializeField] private GameObject inventorySlotUI;
    [SerializeField] private List<PlayerInventorySlotUI> activeInventorySlotUI = new List<PlayerInventorySlotUI>();
    
    [Header("Currently Selected UI")]
    [SerializeField] private Transform currentlySelectedTransform;
    [SerializeField] private AnimationCurve movementCurve;
    [SerializeField] private float movementDuration = 0.5f;
    
    void Awake(){
        InitialiseInventory();
    }

    private void InitialiseInventory(){
        for(int i = 0; i < inventory.ReturnInventoryLimit(); i++){            
            activeInventorySlotUI.Add(Instantiate(inventorySlotUI, this.transform).GetComponent<PlayerInventorySlotUI>());

        }
    }

    public void RefreshUI(){
        for(int i = 0; i < activeInventorySlotUI.Count; i++){
            if(inventory.CheckInventory()[i] != null){
                // Debug.Log($"{inventory.CheckInventory()[i].itemName}");
                activeInventorySlotUI[i].AddItem(inventory.CheckInventory()[i]);
            }
            else{
                activeInventorySlotUI[i].ClearSlot(i);
            }
        }
        // activeInventorySlotUI[inventory.GetFirstEmptySlot()].AddItem(inventory.GetMostRecentlyCollected());
    }

    public void SetActiveUI(){
        // currentlySelectedTransform.position = activeInventorySlotUI[inventory.currentlyEquipped.GetCurrentIndex()].transform.position;

        StartCoroutine(LerpValue(currentlySelectedTransform.position, activeInventorySlotUI[inventory.currentlyEquipped.GetCurrentIndex()].transform.position, movementDuration));
    }

    private IEnumerator LerpValue(Vector3 start, Vector3 end, float duration){
        float timeElapsed = 0f;

        while(timeElapsed < duration){
            float t = timeElapsed / duration;
            t = movementCurve.Evaluate(t);

            var newPos = Vector3.Lerp(start, end, t);

            // Debug.Log($"{newPos}");
            currentlySelectedTransform.position = newPos;

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        currentlySelectedTransform.position = end;
    }
}
