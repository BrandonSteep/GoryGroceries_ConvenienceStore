using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItem : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    private ShoppingListInventory shoppingList;
    [SerializeField] private float throwForce;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameEvent throwEvent;

    void Awake(){
        shoppingList = GameObject.FindWithTag("Shopping List").GetComponent<ShoppingListInventory>();
    }

    public void ThrowCurrentlyHeldItem(){
        if(inventory.GetItemAtIndex(inventory.currentlyEquipped.GetCurrentIndex()) != null){
            ItemSO itemToRemove = inventory.GetItemAtIndex(inventory.currentlyEquipped.GetCurrentIndex());

            var thrownItem = Instantiate(itemToRemove.pickup.gameObject, throwPoint.transform.position, Quaternion.identity);
        
            var rb = thrownItem.AddComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.AddForce(throwPoint.transform.forward * throwForce, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * (throwForce * 0.5f));

            Debug.Log($"Item being removed is {itemToRemove}");

            inventory.RemoveFromInventory(itemToRemove);

            // THE FOLLOWING LINE CAUSES ISSUES PLS FIX <3 //

            if(!inventory.CheckInventory().Contains(itemToRemove) && shoppingList.ContainsItem(itemToRemove)){

                Debug.Log($"{shoppingList.CheckItemStatus(itemToRemove)}");

                if(shoppingList.CheckItemStatus(itemToRemove) != ShoppingItemStatus.Paid){
                    shoppingList.UpdateShoppingListItemStatus(itemToRemove, ShoppingItemStatus.Unchecked);
                }
            }

            throwEvent.Raise();
            StartCoroutine(RemoveRigidBody(thrownItem));
        }
    }

    private IEnumerator RemoveRigidBody(GameObject thrownItem){
        yield return new WaitForSeconds(5f);
        
        if(thrownItem != null){
            Destroy(thrownItem.GetComponent<Rigidbody>());
        }
    }
}
