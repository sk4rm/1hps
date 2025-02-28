using UnityEngine;

public class ChoppableObject : MonoBehaviour
{
    [SerializeField] private float chopDurability;

    public void Chop(float chopEfficiency)
    {
        chopDurability = Mathf.Max(0, chopDurability - chopEfficiency);
        
        if (chopDurability == 0f)
            Debug.Log($"{transform.root.name} is chopped");
    }
}
