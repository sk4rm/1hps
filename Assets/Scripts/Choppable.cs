using System;
using Unity.Netcode;
using UnityEngine;

public class Choppable : NetworkBehaviour, IInteractable
{
    [SerializeField] private float initialChopDurability = 10f;

    private NetworkVariable<float> chopDurability;

    private void Awake()
    {
        chopDurability = new NetworkVariable<float>(initialChopDurability);
    }

    public void Interact()
    {
        ChopRpc(1);
    }

    public event Action<NetworkBehaviourReference> OnChopFinish;
    
    public override void OnNetworkDespawn()
    {
        transform.root.gameObject.SetActive(false);
    }

    [Rpc(SendTo.Server)]
    private void ChopRpc(float chopEfficiency)
    {
        chopDurability.Value = Mathf.Max(0, chopDurability.Value - chopEfficiency);
        if (chopDurability.Value == 0f) OnChopFinish?.Invoke(this);
    }

    public void ResetDurability()
    {
        chopDurability.Value = initialChopDurability;
    }
}