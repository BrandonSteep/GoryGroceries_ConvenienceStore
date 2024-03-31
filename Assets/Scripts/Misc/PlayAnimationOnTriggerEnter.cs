using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationOnTriggerEnter : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private string enterTriggerName;
    [SerializeField] private string exitTriggerName;
    
    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            anim.SetTrigger(enterTriggerName);
        }
    }

    void OnTriggerExit(Collider other){
        if(other.tag == "Player" && exitTriggerName != null){
            anim.SetTrigger(exitTriggerName);
        }
    }

    public void ResetAnimTrigger(int i){
        if(i == 0){
            anim.ResetTrigger(enterTriggerName);
        }
        else{
            anim.ResetTrigger(exitTriggerName);
        }
    }
}
