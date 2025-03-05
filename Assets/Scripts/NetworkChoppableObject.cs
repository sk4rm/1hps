using Unity.Netcode;
using UnityEngine;

public class NetworkChoppableObject : NetworkBehaviour
{
    public delegate void ChopFinishDelegate(NetworkBehaviourReference choppedObject);

    [SerializeField] private float initialChopDurability = 10f;

    private NetworkVariable<float> chopDurability;

    private void Awake()
    {
        chopDurability = new NetworkVariable<float>(initialChopDurability);
    }

    public event ChopFinishDelegate OnChopFinish;

    public override void OnNetworkDespawn()
    {
        transform.root.gameObject.SetActive(false);
    }

    [Rpc(SendTo.Server)]
    public void ChopRpc(float chopEfficiency)
    {
        chopDurability.Value = Mathf.Max(0, chopDurability.Value - chopEfficiency);
        if (chopDurability.Value == 0f) OnChopFinish?.Invoke(this);
    }

    public void ResetDurability()
    {
        chopDurability.Value = initialChopDurability;
    }
}