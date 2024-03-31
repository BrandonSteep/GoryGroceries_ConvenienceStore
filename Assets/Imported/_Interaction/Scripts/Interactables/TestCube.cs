using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCube : MonoBehaviour, IInteractable
{
    [SerializeField] private MeshRenderer rend;
    [SerializeField] private Material white;
    [SerializeField] private Material blue;

    void Awake(){
        rend = GetComponent<MeshRenderer>();
    }

    public void Interact()
    {
        Debug.Log($"Interacting with {this.gameObject.name}");
        Debug.Log($"Material is {rend.material}");
        if(rend.material.name == "White (Instance)"){
            rend.material = blue;
        }
        else{
            rend.material = white;
        }
    }
}
