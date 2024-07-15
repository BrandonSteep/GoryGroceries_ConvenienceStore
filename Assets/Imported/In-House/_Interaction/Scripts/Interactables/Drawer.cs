using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator anim;

    void Start(){
        anim = GetComponent<Animator>();
    }

    public void Interact(){
        anim.SetTrigger("Toggle");
    }
}
