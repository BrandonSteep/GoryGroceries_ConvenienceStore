using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State Machine/State/Chase Player")]
public class State_ChasePlayer : _State
{
    [SerializeField] private float moveSpeed;

    public override _State RunState(StateMachineController stateMachine){
        stateMachine.SetNavDestination(ControllerReferences.player.transform.position);
        return base.RunState(stateMachine);
    }

    public override void EnterState(StateMachineController stateMachine){
        stateMachine.SetNav(true);
    }
}
