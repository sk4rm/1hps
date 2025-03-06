using System;
using UnityEngine;

public class ColliderEventDispatcher : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        OnCollisionEntered?.Invoke(other);
    }

    private void OnCollisionExit(Collision other)
    {
        OnCollisionExited?.Invoke(other);
    }

    private void OnCollisionStay(Collision other)
    {
        OnCollisionStayed?.Invoke(other);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEntered?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExited?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        OnTriggerStayed?.Invoke(other);
    }

    public event Action<Collision> OnCollisionEntered;
    public event Action<Collision> OnCollisionExited;
    public event Action<Collision> OnCollisionStayed;

    public event Action<Collider> OnTriggerEntered;
    public event Action<Collider> OnTriggerExited;
    public event Action<Collider> OnTriggerStayed;
}