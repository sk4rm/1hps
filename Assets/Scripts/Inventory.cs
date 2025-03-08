using Unity.Netcode;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<int> wood = new();

    [Rpc(SendTo.Server)]
    public void AddWoodRpc(int amount)
    {
        wood.Value += amount;
    }
}