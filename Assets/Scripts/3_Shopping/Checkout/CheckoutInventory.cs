using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class CheckoutInventory : MonoBehaviour
{
    [SerializeField] private List<ItemSO> itemsScanned;
    public ItemSO lastScannedItem;

    [Header("Money")]
    [SerializeField] private float checkoutTotal = 0f;
    [SerializeField] private MoneyValueUI totalText;

    [SerializeField] private ScriptableVariable playerMoney;


    [Header("Scan Indicator")]
    [SerializeField] private Renderer[] scanSign;
    [SerializeField] private Material scanGreen;
    [SerializeField] private Material scanRed;
    [SerializeField] private Light scanLight;
    [SerializeField] private GameObject placeItemUI;
    [SerializeField] private GameObject pleaseScanItems;

    [Header("Menu Item UI")]
    [SerializeField] private GameObject menuItem;
    [SerializeField] private RectTransform menuItemLayoutGroup;

    [Header("Bagging")]
    [SerializeField] private Transform baggingArea;
    [SerializeField] private GameObject shoppingBag;
    [SerializeField] private GameObject collectBagsUI;
    
    [Header("Flags")]
    public bool canScan = true;
    public bool hasPaid = false;

    [Header("Effect Events")]
    [SerializeField] private UnityEvent onItemScanned;
    

    void Start(){
        ResetCheckout();
    }

#region Scanning Items

    public void ScanItem(){
        ItemSO itemToScan = ControllerReferences.playerInventory.GetItemAtIndex(ControllerReferences.playerInventory.currentlyEquipped.GetCurrentIndex());

        if(canScan && itemToScan != null){
            AddItem(itemToScan);
            onItemScanned.Invoke();
        }
    }

    private void AddItem(ItemSO scannedItem){
        
        if(scannedItem != null){
            itemsScanned.Add(scannedItem);
            checkoutTotal += scannedItem.price;

            lastScannedItem = scannedItem;

            Debug.Log($"{scannedItem} added to Checkout");
        }

        CannotScan();
        SetUI(scannedItem);
    }

#endregion

#region UI & Indicators
    public void CannotScan(){
        canScan = false;
        foreach(Renderer rend in scanSign){
            rend.material = scanRed;
        }
        scanLight.color = Color.red;
        placeItemUI.SetActive(true);
    }
    public void CanScan(){
        canScan = true;
        foreach(Renderer rend in scanSign){
            rend.material = scanGreen;
        }
        scanLight.color = Color.green;
        placeItemUI.SetActive(false);
    }

    private void ResetUI(){
        foreach(Transform child in menuItemLayoutGroup.transform){
            Destroy(child.gameObject);
        }
        totalText.SetPrice(checkoutTotal);
    }

    private void SetUI(ItemSO scannedItem){
        Instantiate(menuItem, menuItemLayoutGroup).GetComponent<CheckoutMenuItem>().SetDetails(scannedItem);
        totalText.SetPrice(checkoutTotal);
    }
#endregion

#region Checkout Process

    public float CheckoutTotal(){
        return checkoutTotal;
    }

    public void CheckoutFailed(){
        if(checkoutTotal == 0f){
            Debug.Log("No Items Scanned");
            pleaseScanItems.SetActive(true);
        }
        else if(playerMoney.value < checkoutTotal){
            Debug.Log("Insufficient Funds");
            InsufficientFunds();
        }
        else{
            Debug.Log("CHEKOUT FAILED - Debugging Required");
        }
    }

    public void CheckoutSuccessful(){
        var shoppingListInventory = GameObject.FindWithTag("Shopping List").GetComponent<ShoppingListInventory>();
        var otherItemTracker = ControllerReferences.playerInventory.transform.GetComponent<OtherItemTracker>();

        for(int i = 0; i < itemsScanned.Count; i++){
            ShoppingItem shoppingListitem = shoppingListInventory.selectedItems.Find(f => f.itemSO == itemsScanned[i]);
            if(shoppingListitem != null){
                shoppingListInventory.UpdateShoppingListItemStatus(itemsScanned[i], ShoppingItemStatus.Paid);
            }
            else {
                otherItemTracker.AddNewPaidItem(itemsScanned[i]);
            }
        }

        hasPaid = true;
        canScan = false;
        collectBagsUI.SetActive(true);
    }

    public void ResetCheckout(){
        hasPaid = false;
        canScan = true;
        checkoutTotal = 0f;
        ResetUI();
        collectBagsUI.SetActive(false);
        
        var newBag = Instantiate(shoppingBag, baggingArea).GetComponent<CheckoutBag>();
        newBag.checkoutInventory = this;
    }

    private void InsufficientFunds(){

    }

#endregion
}
