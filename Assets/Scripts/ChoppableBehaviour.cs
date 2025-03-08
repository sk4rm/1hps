using System;
using Unity.Netcode;
using UnityEngine;

public class ChoppableBehaviour : NetworkBehaviour
{
    [SerializeField] private float initialChopDurability = 10f;
    [SerializeField] private ItemType resourceType;
    [SerializeField] private int resourceAmount = 1;

    private NetworkVariable<float> chopDurability;

    public ItemType ResourceType => resourceType;
    public int ResourceAmount => resourceAmount;

    private void Awake()
    {
        chopDurability = new NetworkVariable<float>(initialChopDurability);
    }

    public event Action<NetworkBehaviourReference> OnChopFinish;

    public override void OnNetworkDespawn()
    {
        transform.root.gameObject.SetActive(false);
    }

    [Rpc(SendTo.Server)]
    public void ChopRpc(float chopEfficiency)
    {
        chopDurability.Value = Mathf.Max(0, chopDurability.Value - chopEfficiency);

        if (chopDurability.Value == 0f)
        {
            OnChopFinish?.Invoke(this);
            NetworkObject.Despawn(false);
        }
    }

    public void ResetDurability()
    {
        chopDurability.Value = initialChopDurability;
    }
}