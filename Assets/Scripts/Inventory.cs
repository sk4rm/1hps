using Unity.Netcode;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    [field: SerializeField] public NetworkVariable<int> Wood { get; private set; } = new();

    [Rpc(SendTo.Server)]
    public void AddWoodRpc(int amount)
    {
        Wood.Value += amount;
    }
}