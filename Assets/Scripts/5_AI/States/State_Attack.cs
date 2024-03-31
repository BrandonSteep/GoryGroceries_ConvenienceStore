using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "State Machine/State/Attack")]
public class State_Attack : _State
{
    [SerializeField] private string attackName;

    public override _State RunState(StateMachineController stateMachine)
    {
        return base.RunState(stateMachine);
    }

    public override void EnterState(StateMachineController stateMachine)
    {
        stateMachine.SetNav(false);
        stateMachine.SetAnimBool(attackName, true);
        stateMachine.TrackPlayer();
    }

}
