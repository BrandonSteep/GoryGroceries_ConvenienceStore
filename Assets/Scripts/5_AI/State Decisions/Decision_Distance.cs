using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Decision/Distance")]
public class Decision_Distance : StateDecisionSO
{
    [SerializeField] private _State attackState;
    [SerializeField] private _State moveState;
    [SerializeField] private _State idleState;

    [SerializeField] private float attackRange;
    [SerializeField] private float forgetRange;

    public override _State ChooseNextState(StateMachineController stateMachine){
        var dist = Vector3.Distance(stateMachine.transform.position, ControllerReferences.player.transform.position);

        if(dist <= attackRange){
            return attackState;
        }
        else if(dist >= forgetRange){
            return idleState;
        }
        else{
            return moveState;
        }
    }
}
