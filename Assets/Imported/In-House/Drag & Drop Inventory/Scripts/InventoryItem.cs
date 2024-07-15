using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public InventoryManager inventory;

    [Header("UI")]
    public Image image;
    public Text countText;

    [HideInInspector] public Transform parentBeforeDrag;
    [HideInInspector] public Transform parentAfterDrag;
    
    public int count = 1;
    public Item item;

    // Keeps Track of whether the held Stack was Split
    public bool split;

    void Start(){
        inventory = GameObject.FindWithTag("Inventory").GetComponent<InventoryManager>();
    }
    
    public void InitializeItem(Item newItem){
        item = newItem;
        image.sprite =newItem.image;
        
        RefreshCount();
    }

    public void RefreshCount(){
        countText.text = count.ToString();
        bool textActive = item.stackable;
        countText.gameObject.SetActive(textActive);
    }

    // Drag & Drop
    #region Drag & Drop
    public void OnBeginDrag(PointerEventData eventData){
        parentAfterDrag = transform.parent;

        if(eventData.button == PointerEventData.InputButton.Right){
            // Debug.Log("Right Mouse Drag");
            SplitStack();
        }
        
        image.raycastTarget = false;
        transform.SetParent(transform.root);
        countText.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData){
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData){
        image.raycastTarget = true;
        split = false;

        if(parentAfterDrag.childCount == 0){
            transform.SetParent(parentAfterDrag);
        }
        else{
            InventoryItem other = parentAfterDrag.transform.GetChild(0).GetComponent<InventoryItem>();

            // Calculate which figure is smallest between this count item's count, and the space remaining in the other
            int countTilOtherStackFull = other.item.maxStackedItems - other.count;
            int transferAmount = Mathf.Min(count, countTilOtherStackFull);

            // Add the calculated transfer amount to the new item
            other.count += transferAmount;
            other.RefreshCount();

            if(count > transferAmount){
                // There is more in this stack than the other
                // Remove the transfer amount and put it back
                count -= transferAmount;
                RefreshCount();                
            }
            else{
                // This item is now empty
                // Destroy this GameObject
                Destroy(this.gameObject);                
            }
        }
    
        countText.raycastTarget = true;
    }
    #endregion

    // Split Stack Behaviour
    private void SplitStack(){
        if(item.stackable && count > 1){
            Debug.Log("Split Stack");
            split = true;

            int halfValue = count/2;
            int remainder = count%2;

            int firstHalf = halfValue;
            int secondHalf = halfValue;

            if(remainder == 1){
                firstHalf += 1;
            }

            inventory.SpawnNewItem(item, parentAfterDrag.GetComponent<InventorySlot>(), secondHalf);
            count = firstHalf;
            RefreshCount();
        }
    }
}
