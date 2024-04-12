using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ShoppingListInventory : MonoBehaviour
{
    [SerializeField] private AvailableShoppingListItemsSO availableItems;
    [SerializeField] private AvailableShoppingListItemsSO manualItems;
    public List<ShoppingItem> selectedItems = new List<ShoppingItem>();
    public int numberOfListItems;

    [Header("UI")]
    [SerializeField] private ShoppingListUISetter uiSetter;

#region Enable & Disable
    void OnEnable()
    {
        if(!uiSetter){
            uiSetter = GetComponent<ShoppingListUISetter>();
        }

        if(numberOfListItems > availableItems.availableItems.Length){
            Debug.LogWarning($"Fewer Shopping Items than Number Requests - Reducing Item Count to {availableItems.availableItems.Length}");
            numberOfListItems = availableItems.availableItems.Length;
            GenerateNewList();
        }
        else{
            if(manualItems != null){
                // Fill List Manually
                Debug.LogWarning("Manual Shopping List Selected");
            }
            else{
                GenerateNewList();
            }
        }
    }

    void OnDisable(){
        ClearList();
    }
#endregion

#region Create & Clear Shopping List

    private void UseManualList(){
        selectedItems.AddRange(availableItems.availableItems);
        FinaliseList();
    }

    private void GenerateNewList(){
        int[] pickedIndexes = new int[numberOfListItems];
        HashSet<int> pickedIndexesSet = new HashSet<int>();
        
        for(int i = 0; i<numberOfListItems; i++){
            int newIndex = GetUniqueRandomIndex(pickedIndexesSet);
            pickedIndexes[i] = newIndex;
            pickedIndexesSet.Add(newIndex);
            
            selectedItems.Add(new ShoppingItem(availableItems.availableItems[pickedIndexes[i]], ShoppingItemStatus.Unchecked));
        }

        // Debug.Log for Chosen Indexes
        // Debug.Log($"Selected Indexes = {string.Join(",", pickedIndexes)}");

        FinaliseList();
    }

    private int GetUniqueRandomIndex(HashSet<int> pickedIndexesSet){
        int newIndex;
        do{
            newIndex = Random.Range(0, availableItems.availableItems.Length);
        }
        while(pickedIndexesSet.Contains(newIndex));

        return newIndex;
    }

    private void FinaliseList(){
        SetGameManagerItemList();
        uiSetter.AddListItems(selectedItems);
    }

    private void ClearList(){
        selectedItems.Clear();
    }

#endregion

    public void UpdateShoppingListItemStatus(ItemSO item, ShoppingItemStatus newStatus){
        // Debug.Log($"{item.itemName}");
        var itemToUpdate = selectedItems.Find(i => i.itemSO.itemName == item.itemName);

        if(itemToUpdate != null){
            Debug.Log($"Setting {itemToUpdate.itemSO.itemName} as {newStatus}");
            if(itemToUpdate.status != newStatus){
                itemToUpdate.status = newStatus;
            }
        }
    }

    private void SetGameManagerItemList(){
        GameManager.currentItems.Clear();
        for(int i = 0; i < selectedItems.Count; i++){
            GameManager.currentItems.Add(selectedItems[i]);
        }
    }

    public ShoppingItemStatus CheckItemStatus(ItemSO item){

        if(ContainsItem(item)){
            ShoppingItemStatus status = selectedItems.Find(i => i.itemSO.itemName == item.itemName).status;
            Debug.Log($"Item is currently set as {status}");
            return status;
        }
        else return ShoppingItemStatus.Unchecked;        
    }

    public bool ContainsItem(ItemSO item){
        ShoppingItem foundItem = selectedItems.Find(i => i.itemSO == item);
        if(foundItem != null){
            return true;
        }
        else return false;
    }

    public List<ShoppingItem> CheckShoppingList(){
        return selectedItems;
    }
}
