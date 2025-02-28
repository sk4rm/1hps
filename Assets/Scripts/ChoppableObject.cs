using System;
using UnityEngine;

public class ChoppableObject : MonoBehaviour
{
    public delegate void ChopFinishDelegate(ChoppableObject choppedObject);

    public event ChopFinishDelegate OnChopFinish;

    [SerializeField] private float initialChopDurability;

    private float chopDurability;

    private void Awake()
    {
        chopDurability = initialChopDurability;
    }

    public void Chop(float chopEfficiency)
    {
        chopDurability = Mathf.Max(0, chopDurability - chopEfficiency);

        if (chopDurability == 0f) OnChopFinish?.Invoke(this);
    }

    public void ResetDurability()
    {
        chopDurability = initialChopDurability;
    }
}