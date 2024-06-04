using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(menuName = "State Machine/State/Wander")]
public class State_Wander : _State
{    
    private Vector3 _wanderDestination;
    private bool _getNewPosition;

    [SerializeField] private float _destinationRange;
    [SerializeField] private float _timeBetweenNewDestination;

    public override _State RunState(StateMachineController stateMachine){
        stateMachine.SetNav(true);
        stateMachine.SetNavDestination(_wanderDestination);

        if(stateMachine.TriggerAfterTimeElapsed(_timeBetweenNewDestination)){
            _wanderDestination = GetRandomPosition(stateMachine);
        }

        float distanceToWanderDestination = Vector3.Distance(_wanderDestination, stateMachine.transform.position);
        if(distanceToWanderDestination < 1f){
            _wanderDestination = GetRandomPosition(stateMachine);
        }

        return base.RunState(stateMachine);
    }

    Vector3 GetRandomPosition(StateMachineController stateMachine){
        Vector3 randDirection = Random.insideUnitSphere * _destinationRange;

        randDirection += stateMachine.transform.position;

        UnityEngine.AI.NavMeshHit navHit;
        UnityEngine.AI.NavMesh.SamplePosition(randDirection, out navHit, _destinationRange, -1);

        stateMachine.LogMessage($"{navHit.position}");
        return navHit.position;
    }


#region Initialisation
    public override void EnterState(StateMachineController stateMachine){
        _wanderDestination = GetRandomPosition(stateMachine);
        base.EnterState(stateMachine);
    }

    public override void ExitState(StateMachineController stateMachine){

    }
    #endregion
}
