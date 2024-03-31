using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationController : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private int maxIdleAnims;
    [SerializeField] private int maxWalkAnims;

    void Awake(){
        anim = GetComponent<Animator>();
        SetRandomIdleAnim();
        SetRandomWalkAnim();
    }

    public void SetRandomIdleAnim(){
        anim.SetInteger("IdleIndex", Random.Range(0, maxIdleAnims));
    }

    public void SetRandomWalkAnim(){
        anim.SetInteger("WalkIndex", Random.Range(0, maxWalkAnims));
    }

    public void ResetGrab(){
        anim.ResetTrigger("Grab");
    }

    public void SetBool(string name, bool value){
        anim.SetBool(name, value);
    }
}
