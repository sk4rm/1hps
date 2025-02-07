using Unity.Netcode;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<int> health;
    private const int InitialHealth = 1;

    public override void OnNetworkSpawn()
    {
        health.Value = InitialHealth;
    }

    public void TakeDamage(int damage)
    {
        health.Value -= damage;
    }
}
