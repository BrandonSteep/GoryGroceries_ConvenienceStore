using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Status, IStatus
{
    public void TakeDamage()
    {
        if (canTakeDamage)
        {
            ControllerReferences.playerAnim.SetTrigger("TakeDamage");
            canTakeDamage = false;
        }
    }

    public void ResetDamage()
    {
        canTakeDamage = true;
    }
    
    
    
    public void Die()
    {

    }
}
