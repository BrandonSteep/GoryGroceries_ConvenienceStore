using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPhysicsHandler : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private float mass = 1f;

    [SerializeField] private float rbDisableTimer = 1f;
    [SerializeField] private float rbDisableTimerCurrent = 1f;

    [SerializeField] private bool physicsOnEnable;

    // Start is called before the first frame update
    void Start(){
        _rb = GetComponent<Rigidbody>();
        ResetTimer();
    }

    // Update is called once per frame
    void Update(){
        if (_rb != null){
            RigidbodyDisabler();
        }
    }

    void OnEnable(){
        if(physicsOnEnable){
            RigidbodyEnabler();
        }
    }

    private void RigidbodyEnabler(){
        _rb = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        _rb.mass = mass;
    }

    private void RigidbodyDisabler(){
        if (_rb.velocity.magnitude < 0.5f){
            rbDisableTimerCurrent -= Time.deltaTime;
            if (rbDisableTimerCurrent <= 0f){
                Destroy(_rb);
                Debug.Log("Rigidbody Disabled");

            }
        }
        else{
            ResetTimer();
        }
    }

    private void ResetTimer(){
        rbDisableTimerCurrent = rbDisableTimer;
    }

    #region Forces
    public void AddForce(Vector3 force, ForceMode mode){
        ResetTimer();
        if(_rb == null){
            RigidbodyEnabler();
        }
        _rb.AddForce(force, mode);
    }

    public void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius, float upwardsModifier, ForceMode mode){
        ResetTimer();
        if (_rb == null)
        {
            RigidbodyEnabler();
        }
        _rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius, upwardsModifier, mode);
    }
    #endregion
}
