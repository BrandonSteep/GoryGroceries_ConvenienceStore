using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status, IStatus
{
    private PlayerGrabbed grabbable;
    [Tooltip("This value is set in Seconds")][SerializeField] private float iFrames = 1f;

    void Start(){
        grabbable = GetComponent<PlayerGrabbed>();
    }

    public void TakeDamage()
    {
        ControllerReferences.playerAnim.SetTrigger("TakeDamage");
        currentHp.value -= 25f;
        canTakeDamage = false;
    }

    public void ResetDamage()
    {
        canTakeDamage = true;
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Grab Trigger" && canTakeDamage){
            canTakeDamage = false;
            Debug.Log("Player Grabbed!");
            grabbable.GrabbedByZombie(other);
        }
    }

    public float GetIFrames(){
        return iFrames;
    }

    public void AddIFrames(){
        canTakeDamage = false;
        Invoke("ResetDamage", iFrames);
    }
    
    public void Die()
    {

    }
}
