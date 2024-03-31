using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status, IStatus
{
    [SerializeField] private Behaviour[] behavioursToDisableOnGrab;
    [SerializeField] private float iFrames = 1f;

    public void TakeDamage()
    {
        if (canTakeDamage)
        {
            ControllerReferences.playerAnim.SetTrigger("TakeDamage");
            currentHp.value -= 25f;
            canTakeDamage = false;
        }
    }

    public void ResetDamage()
    {
        canTakeDamage = true;
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Grab Trigger" && canTakeDamage){
            GrabbedByZombie();
        }
    }

#region Zombie Grab Logic
    public void GrabbedByZombie(){
        foreach(Behaviour i in behavioursToDisableOnGrab){
            i.enabled = false;
        }

        // Move Player Into Position
        // Make Player Look At Zombie
    }

    public void EndGrab(){
        foreach(Behaviour i in behavioursToDisableOnGrab){
            i.enabled = true;
        }

        ControllerReferences.playerKnockback.AddImpact(-transform.forward, 15f);
        Invoke("ResetDamage", iFrames);
    }
#endregion
    
    public void Die()
    {

    }
}
