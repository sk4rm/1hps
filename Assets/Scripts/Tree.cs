using System;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [field: SerializeField] public ChoppableBehaviour Choppable { get; private set; }
    [SerializeField] private Transform pivot;
    [SerializeField] private float visualFeedbackDamping = 1f;
    
    private Vector3 originalScale = Vector3.one;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        Choppable.OnChop += OnChop;
    }

    private void OnDisable()
    {
        Choppable.OnChop -= OnChop;
    }

    private void OnChop()
    {
        pivot.localScale = 0.9f * originalScale;
    }

    private void Update()
    {
        pivot.localScale = Vector3.Slerp(pivot.localScale, originalScale, Time.deltaTime * visualFeedbackDamping);
    }
}
