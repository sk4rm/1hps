using UnityEngine;

public class ChoppableObject : MonoBehaviour
{
    public delegate void OnChopDelegate(float chopAmount);
    public event OnChopDelegate OnChop;
    
    public delegate void OnChopFinishDelegate();
    public event OnChopFinishDelegate OnChopFinish;
    
    [SerializeField] private float chopDurability;

    public void Chop(float chopEfficiency)
    {
        var newDurability = chopDurability - chopEfficiency;
        OnChop?.Invoke(newDurability - chopDurability);
        
        if (newDurability < 0)
        {
            chopDurability = 0;
            OnChopFinish?.Invoke();
        }
    }
}
