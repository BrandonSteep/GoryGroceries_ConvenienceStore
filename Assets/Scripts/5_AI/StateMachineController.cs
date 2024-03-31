using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class StateMachineController : MonoBehaviour
{
    [SerializeField] private _State startState;
    [SerializeField] private _State currentState;
    [SerializeField] private _State moveState;
    
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
    // [SerializeField] private AnimationCurve curve;
    [SerializeField] private float lookAtSpeed;
    private Coroutine LookCoroutine;

    public void TrackPlayer(){


        if(LookCoroutine != null){
            StopCoroutine(LookCoroutine);
        }
        LookCoroutine = StartCoroutine(LookAt());
    }

    private IEnumerator LookAt(){
        
        Debug.Log("Tracking Player for Attack");


        Vector3 lookPosition = new Vector3(ControllerReferences.player.transform.position.x, transform.position.y, ControllerReferences.player.transform.position.z);

        Quaternion lookRotation = Quaternion.LookRotation(lookPosition - transform.position);

        float time = 0f;

        while(time < 1){
            Quaternion newRotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            transform.rotation = newRotation;

            time += Time.deltaTime * lookAtSpeed;
            yield return null;
        }

        nav.enabled = true;
    }
#endregion
}
