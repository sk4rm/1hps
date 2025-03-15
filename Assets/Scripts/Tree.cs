using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] private ChoppableBehaviour choppable;
    [SerializeField] private Transform pivot;
    [SerializeField] private float visualFeedbackDamping = 1f;
    
    private Vector3 originalScale = Vector3.one;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        choppable.OnChop += OnChop;
    }

    private void OnDisable()
    {
        choppable.OnChop -= OnChop;
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
