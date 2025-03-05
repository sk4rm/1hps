using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerName : NetworkBehaviour
{
    [SerializeField] private string defaultDisplayName = "john lumberjack";

    [SerializeField] private NetworkVariable<FixedString64Bytes> displayName = new(string.Empty,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] private TextMeshProUGUI nameTag;

    public string DisplayName
    {
        get => displayName.Value.ToString();
        set => displayName.Value = new FixedString64Bytes(value);
    }

    public override void OnNetworkSpawn()
    {
        displayName.OnValueChanged += UpdateNameTag;

        displayName.OnValueChanged.Invoke(displayName.Value, defaultDisplayName);
    }

    public override void OnNetworkDespawn()
    {
        displayName.OnValueChanged -= UpdateNameTag;
    }

    private void UpdateNameTag(FixedString64Bytes previousValue, FixedString64Bytes newValue)
    {
        nameTag.text = newValue.ToString();
    }
}