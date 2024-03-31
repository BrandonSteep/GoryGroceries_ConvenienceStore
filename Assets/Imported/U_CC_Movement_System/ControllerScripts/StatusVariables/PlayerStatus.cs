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
            GrabbedByZombie(other);
        }
    }

#region Zombie Grab Logic
    public void GrabbedByZombie(Collider other){
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

    // Smooth Damp Interpolation for Zombie Grab Movement
    [SerializeField] private AnimationCurve interpolationCurve;
    [SerializeField] private float interpolationDuration = 0.75f;
    private IEnumerator LerpValue(Vector3 endPosition){
        float timeElapsed = 0f;

        while(timeElapsed < interpolationDuration){
            float t = timeElapsed / interpolationDuration;
            t = interpolationCurve.Evaluate(t);

            var newPos = Vector3.Lerp(this.transform.position, endPosition, t);

            // Debug.Log($"{newPos}");
            transform.position = newPos;

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = ControllerReferences.player.transform.position;
    }
#endregion
    
    public void Die()
    {

    }
}
