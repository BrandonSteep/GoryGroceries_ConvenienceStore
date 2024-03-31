using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/Decision/Sight and Hearing")]
public class SightAndHearingCheck : StateDecisionSO
{
    public override _State ChooseNextState(StateMachineController stateMachine)
    {
        Debug.Log("Performing Sight and Hearing Check");
        return base.ChooseNextState(stateMachine);
    }
}
