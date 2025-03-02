using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class NetworkPlayerNameTag : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        GameManager.Instance.TryGetPlayerName(NetworkManager.Singleton.LocalClientId, out var playerName);   
        GetComponent<TextMeshProUGUI>().text = playerName;
    }
}