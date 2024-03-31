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
    [SerializeField] private Transform focalPoint;
    
    private NavMeshAgent nav;
    private ZombieAnimationController anim;

    private FieldOfView fieldOfView;

    #region Core State Machine Functionality
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
    
    public _State CheckCurrentState(){
        return currentState;
    }
    #endregion

    #region Set Specific States
    public void SetMoveState(){
        TransitionState(moveState);
    }

    public void SetBiteState(){
        TransitionState(biteState);
    }
    #endregion

    #region Navigation
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

    public void SetNavDestination(Vector3 destPos){
        nav.SetDestination(destPos);
    }
    #endregion

    #region Track Player
    [SerializeField] private float lookAtSpeed;

    public void TrackPlayer(){
        Debug.Log("Tracking Player for Attack");

        Vector3 lookPosition = new Vector3(ControllerReferences.player.transform.position.x, transform.position.y, ControllerReferences.player.transform.position.z);
        lookAt.StartLookAt(this.transform, lookAtSpeed, lookPosition);
        
        nav.enabled = true;
    }

    public void MoveToPlayer(){
        SetNav(false);
        LerpPosition(ControllerReferences.player.transform.position + ControllerReferences.player.transform.forward * 2f);
    }
    
    [SerializeField] private AnimationCurve interpolationCurve;
    private IEnumerator LerpPosition(Vector3 endPosition){
        float timeElapsed = 0f;

        while(timeElapsed < 0.25f){
            float t = timeElapsed / 0.25f;
            t = interpolationCurve.Evaluate(t);

            var newPos = Vector3.Lerp(transform.position, endPosition, t);

            // Debug.Log($"{newPos}");
            transform.position = newPos;

            timeElapsed += Time.deltaTime;

            yield return null;
        }
        Debug.Log($"Setting Zombie Position to in front of the Player");
        transform.position = endPosition;
    }

    #endregion

    #region Access Methods
    public void SetAnimBool(string name, bool value){
        anim.SetBool(name, value);
    }

    public void SetAnimTrigger(string name){
        anim.SetTrigger(name);
    }

    public _State CheckFOV(_State successState, _State failState){
        return fieldOfView.CheckFOV(this, successState, failState);
    }

    public Transform CheckFocalPoint(){
        return focalPoint;
    }
    #endregion
}
