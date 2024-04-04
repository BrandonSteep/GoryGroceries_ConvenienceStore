using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/State/Idle")]
public class State_Idle : _State
{
    public override _State RunState(StateMachineController stateMachine){
        return base.RunState(stateMachine);
    }

    public override void EnterState(StateMachineController stateMachine)
    {
        stateMachine.SetNav(false);
    }
}
