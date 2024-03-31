using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class StateMachineController : MonoBehaviour
{
    [SerializeField] private _State startState;
    [SerializeField] private _State currentState;
    [SerializeField] private _State moveState;
    [SerializeField] private _State biteState;
    [SerializeField] private LookAt lookAt;
    
    private NavMeshAgent nav;
    private ZombieAnimationController anim;

    private FieldOfView fieldOfView;

    void Start(){
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<ZombieAnimationController>();
        fieldOfView = GetComponent<FieldOfView>();

        TransitionState(startState);

        InvokeRepeating("RunState", 0f, 0.15f);
    }

    void RunState(){
        _State nextState = currentState.RunState(this);
        
        if(nextState != currentState){
            Debug.Log($"Transitioning from {currentState} to {nextState}");
            TransitionState(nextState);
        }
    }

    private void TransitionState(_State nextState){
        currentState = nextState;
        currentState.EnterState(this);
    }

    public void SetMoveState(){
        TransitionState(moveState);
    }

    public void SetBiteState(){
        TransitionState(biteState);
    }

    public void SetNav(bool active){
        if(active){
            nav.enabled = true;
            nav.speed = .1f;
            anim.SetBool("PlayerSeen", true);
        }
        else{
            nav.enabled = false;
            nav.speed = 0f;
            anim.SetBool("PlayerSeen", false);
        }
    }

    public void SetAnimBool(string name, bool value){
        anim.SetBool(name, value);
    }

    public void SetNavDestination(Vector3 destPos){
        nav.SetDestination(destPos);
    }

    public _State CheckCurrentState(){
        return currentState;
    }

    public _State CheckFOV(_State successState, _State failState){
        return fieldOfView.CheckFOV(this, successState, failState);
    }



#region Track Player

    [SerializeField] private float lookAtSpeed;

    public void TrackPlayer(){
        Debug.Log("Tracking Player for Attack");

        lookAt.StartLookAt(lookAtSpeed);
        
        nav.enabled = true;
    }
#endregion
}
