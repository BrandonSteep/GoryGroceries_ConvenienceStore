using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class StateMachineController : MonoBehaviour
{
    [SerializeField] private _State[] startStates;
    [SerializeField] private _State currentState;
    [SerializeField] private _State moveState;
    [SerializeField] private _State biteState;
    [SerializeField] private LookAt lookAt;
    [SerializeField] private Transform focalPoint;

    [SerializeField] private float minMoveSpeed = 1f;
    [SerializeField] private float maxMoveSpeed = 1f;

    private float timeInState = 0f;
    
    private NavMeshAgent nav;
    private ZombieAnimationController anim;

    private FieldOfView fieldOfView;

    #region Core State Machine Functionality
    void Start(){
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<ZombieAnimationController>();
        anim.SetFloat("MoveSpeed", Random.Range(minMoveSpeed, maxMoveSpeed));

        fieldOfView = GetComponent<FieldOfView>();

        int i = 0;
        if(startStates.Count() > 1){
            i = Random.Range(0, startStates.Count());
        }

        Debug.Log(i);

        TransitionState(startStates[i]);
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
        if(currentState != null){
            currentState.ExitState(this);
        }
        currentState = nextState;
        currentState.EnterState(this);
    }
    
    public _State CheckCurrentState(){
        return currentState;
    }

    public bool TriggerAfterTimeElapsed (float triggerDuration){
        timeInState += Time.deltaTime * 10;
        if(timeInState >= triggerDuration){
            timeInState = 0f;
            return true;
        }
        else return false;
    }

    public void LogMessage(string message){
        Debug.Log(message);
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

    public void MoveToPlayer(Transform movePosition){
        SetNav(false);
        StartCoroutine(LerpPosition(movePosition));
    }
    
    [SerializeField] private AnimationCurve interpolationCurve;
    [SerializeField] private float interpolationDuration;
    private IEnumerator LerpPosition(Transform endTransform){
        yield return new WaitForSeconds(0.25f);

        Vector3 endPosition = new Vector3(endTransform.position.x, this.transform.position.y, endTransform.position.z);
        TrackPlayer();

        float timeElapsed = 0f;

        while(timeElapsed < interpolationDuration){
            float t = timeElapsed / interpolationDuration;
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
