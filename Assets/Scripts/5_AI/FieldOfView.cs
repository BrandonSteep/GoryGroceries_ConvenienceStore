using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private float radius;
    [Range(0,360)] [SerializeField] private float angle;
    private bool canSeePlayer;
    
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstructionMask;

    public _State CheckFOV(StateMachineController stateMachine, _State successState, _State failState){

        var thisPosition = stateMachine.transform.position + Vector3.up * 1f;

        Collider[] rangeChecks = Physics.OverlapSphere(thisPosition, radius, targetMask);

        if(rangeChecks.Length != 0){
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - thisPosition).normalized;

            if(Vector3.Angle(transform.forward, directionToTarget) < angle * 0.5){
                Debug.Log("Running Zombie FOV");

                float distanceToTarget = Vector3.Distance(thisPosition, target.position);

                if(!Physics.Raycast(thisPosition, directionToTarget, distanceToTarget, obstructionMask)){
                    
                    Debug.Log("FoV Success!");
                    canSeePlayer = true;
                    return successState;
                }
                else{
                    canSeePlayer = false;
                    return failState;
                }
            }
            else{
                canSeePlayer = false;
                return failState;
            }
        }
        else{
            canSeePlayer = false;
            return failState;
        }
    }

    public float CheckRadius(){
        return radius;
    }

    public float CheckAngle(){
        return angle;
    }

    public bool CheckPlayerSight(){
        return canSeePlayer;
    }
}
