using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Decision/FieldOfView")]
public class Decision_FieldOfView : StateDecisionSO
{
    [SerializeField] private _State successState;
    [SerializeField] private _State failState;
    
    public override _State ChooseNextState(StateMachineController stateMachine){
        return stateMachine.CheckFOV(successState, failState);
    }

}
