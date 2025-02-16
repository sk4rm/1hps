using System;
using UnityEngine;

public class SlugAI : MonoBehaviour
{
    [SerializeField] private State currentState = State.Idle;
    [SerializeField] private ColliderEventDispatcher maxChaseRegion;

    [Header("Chasing Options")] [SerializeField]
    private GameObject chaseTarget;

    private enum State
    {
        Idle,
        Chasing
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

    private void UpdateIdle()
    {
    }

    private void UpdateChasing()
    {
        transform.LookAt(chaseTarget.transform);
        var lookAngles = transform.eulerAngles;
        lookAngles.x = 0;
        lookAngles.z = 0;
        transform.eulerAngles = lookAngles;
    }
}