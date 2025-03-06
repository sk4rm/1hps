using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<float> health = new(0, default, NetworkVariableWritePermission.Owner);

    public void TakeDamage(float damage)
    {
        if (!HasAuthority) return;
        health.Value -= damage;
    }
}