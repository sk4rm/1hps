using System;
using UnityEngine;

public class ColliderEventDispatcher : MonoBehaviour
{
    public delegate void OnCollisionDelegate(Collision other);

    public delegate void OnTriggerDelegate(Collider other);

    public event OnCollisionDelegate OnCollisionEntered;
    public event OnCollisionDelegate OnCollisionExited;
    public event OnCollisionDelegate OnCollisionStayed;

    public event OnTriggerDelegate OnTriggerEntered;
    public event OnTriggerDelegate OnTriggerExited;
    public event OnTriggerDelegate OnTriggerStayed;

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
}