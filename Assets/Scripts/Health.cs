using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    private const int InitialHealth = 1;
    [SerializeField] private NetworkVariable<int> health;

    public override void OnNetworkSpawn()
    {
        health.Value = InitialHealth;
    }

    public void TakeDamage(int damage)
    {
        health.Value -= damage;
    }
}