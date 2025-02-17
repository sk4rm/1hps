using System;
using UnityEngine;

public class SlugAI : MonoBehaviour
{
    [SerializeField] private new Rigidbody rigidbody;
    
    [Header("State Machine")] [SerializeField]
    private State currentState = State.Idle;

    [SerializeField] private ColliderEventDispatcher maxChaseRegion;

    [Header("Chasing-specific Options")] [SerializeField]
    private GameObject chaseTarget;

    [SerializeField] private float lookDamping = 5f;

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                UpdateIdle();
                break;
            case State.Chasing:
                UpdateChasing();
                break;
            default:
                throw new ArgumentOutOfRangeException($"unreachable state: {currentState}");
        }
    }

    private void OnEnable()
    {
        maxChaseRegion.OnTriggerEntered += ConsiderChase;
        maxChaseRegion.OnTriggerExited += ConsiderStopChasing;
    }

    private void OnDisable()
    {
        maxChaseRegion.OnTriggerEntered -= ConsiderChase;
        maxChaseRegion.OnTriggerExited -= ConsiderStopChasing;
    }

    private void ConsiderChase(Collider other)
    {
        var shouldChase = other.transform.root.gameObject.TryGetComponent<NetworkPlayerMovement>(out _);
        if (!shouldChase) return;

        StartChasing(other.gameObject);
    }

    private void ConsiderStopChasing(Collider other)
    {
        if (currentState != State.Chasing) return;
        if (other.gameObject != chaseTarget) return;

        StopChasing();
    }

    private void StartChasing(GameObject target)
    {
        currentState = State.Chasing;
        chaseTarget = target;
    }

    private void StopChasing()
    {
        currentState = State.Idle;
        chaseTarget = null;
    }

    private void UpdateIdle()
    {
    }

    private void UpdateChasing()
    {
        LookAt(chaseTarget.transform);
    }

    private void LookAt(Transform target)
    {
        var lookAngles = Quaternion.LookRotation(target.position - transform.position);
        lookAngles.x = 0;
        lookAngles.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAngles, Time.deltaTime * lookDamping);
    }

    private enum State
    {
        Idle,
        Chasing
    }
}