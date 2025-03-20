using Unity.Netcode;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    [field: SerializeField] public NetworkVariable<int> Money { get; private set; } = new();
    [field: SerializeField] public NetworkVariable<int> Wood { get; private set; } = new();

    [Rpc(SendTo.Server)]
    public void AddWoodRpc(int amount)
    {
        Wood.Value += amount;
    }

    [Rpc(SendTo.Server)]
    public void SellWoodRpc(int moneyPerWood)
    {
        if (Wood.Value <= 0) return;
        Money.Value += Wood.Value * moneyPerWood;
        Wood.Value = 0;
    }
}