using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public event Action OnInteract;
    
    public void Interact()
    {
        OnInteract?.Invoke();
    }
}