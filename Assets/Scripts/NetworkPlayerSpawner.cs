using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject networkPlayerPrefab;
    [SerializeField] private Vector3 spawnPosition;

    public override void OnNetworkSpawn()
    {
        // Debug.Log($"IsOwner: {IsOwner}, IsHost: {IsHost}, IsClient: {IsClient}, IsServer: {IsServer}");

        SpawnPlayerRpc();
    }

    [Rpc(SendTo.Server)]
    private void SpawnPlayerRpc(RpcParams rpcParams = default)
    {
        var senderId = rpcParams.Receive.SenderClientId;

        var player = Instantiate(networkPlayerPrefab);
        player.transform.position = spawnPosition;

        var networkObject = player.GetComponent<NetworkObject>();
        networkObject.SpawnWithOwnership(senderId);
        Debug.Log($"Spawned player {senderId} into the world!");

        SetCinemachineTargetRpc(networkObject.NetworkObjectId, senderId);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SetCinemachineTargetRpc(ulong networkObjectId, ulong senderClientId)
    {
        if (senderClientId != NetworkManager.Singleton.LocalClientId)
            return;

        NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out var playerNetworkObject);
        if (playerNetworkObject == null)
        {
            Debug.LogWarning("Unable to locate spawned player network object.");
            return;
        }

        var cinemachineCamera = FindAnyObjectByType<CinemachineCamera>();
        // var pivot = playerNetworkObject.transform.Find("Pivot");
        cinemachineCamera.Follow = playerNetworkObject.transform;
    }
}