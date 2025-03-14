using Unity.Netcode;

public class Inventory : NetworkBehaviour
{
    public NetworkVariable<int> Wood { get; } = new();

    [Rpc(SendTo.Server)]
    public void AddWoodRpc(int amount)
    {
        Wood.Value += amount;
    }
}