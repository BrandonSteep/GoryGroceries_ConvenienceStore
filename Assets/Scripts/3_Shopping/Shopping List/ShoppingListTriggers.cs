using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingListTriggers : MonoBehaviour
{
    private Animator anim;

    void Start(){
        anim = GetComponent<Animator>();
    }

    public void ToggleShoppingList(){
        anim.SetTrigger("Toggle");
    }
}
