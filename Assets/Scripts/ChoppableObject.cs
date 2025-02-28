using UnityEngine;

public class ChoppableObject : MonoBehaviour
{
    public delegate void ChopFinishDelegate(ChoppableObject choppedObject);

    [SerializeField] private float chopDurability;

    public event ChopFinishDelegate OnChopFinish;

    public void Chop(float chopEfficiency)
    {
        chopDurability = Mathf.Max(0, chopDurability - chopEfficiency);

        if (chopDurability == 0f) OnChopFinish?.Invoke(this);
    }
}