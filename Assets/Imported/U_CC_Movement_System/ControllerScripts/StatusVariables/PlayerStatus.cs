using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status, IStatus
{
    [SerializeField] private Component[] componentsToDisableOnGrab;

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

    
    public void GrabbedByZombie(){

    }
    
    
    public void Die()
    {

    }
}
