using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerName : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<FixedString64Bytes> displayName = new();
    [SerializeField] private TextMeshProUGUI nameTag;

    public override void OnNetworkSpawn()
    {
        displayName.OnValueChanged += UpdateNameTag;

        if (IsOwner) SetDisplayNameRpc(GameManager.Instance.localPlayerDisplayName);

        UpdateNameTag(displayName.Value, displayName.Value);
    }

    public override void OnNetworkDespawn()
    {
        displayName.OnValueChanged -= UpdateNameTag;
    }

    private void UpdateNameTag(FixedString64Bytes previousValue, FixedString64Bytes newValue)
    {
        if (nameTag == null) return;
        nameTag.text = newValue.ToString();
    }

    [Rpc(SendTo.Server)]
    private void SetDisplayNameRpc(string newName)
    {
        displayName.Value = new FixedString64Bytes(newName);
    }
}