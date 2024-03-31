using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimTrigger : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private string triggerName1;
    [SerializeField] private string triggerName2;

    void Awake(){
        if(!anim){
            anim = GetComponent<Animator>();
        }
    }

    public void AnimTriggerReset(int i){
        if(i == 0){
            anim.ResetTrigger(triggerName1);
        }
        else{
            anim.ResetTrigger(triggerName2);
        }
    }

    public void ResetBothAnimTriggers(){
        anim.ResetTrigger(triggerName1);
        anim.ResetTrigger(triggerName2);
    }
}
