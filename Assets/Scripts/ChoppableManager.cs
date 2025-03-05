using Unity.Netcode;
using UnityEngine;

public class ChoppableManager : NetworkBehaviour
{
    public static ChoppableManager Instance;

    [SerializeField] private NetworkChoppableObject[] choppableObjects;

    public override void OnNetworkSpawn()
    {
        #region Singleton

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        #endregion

        choppableObjects = FindObjectsByType<NetworkChoppableObject>(FindObjectsSortMode.None);
        foreach (var choppable in choppableObjects) choppable.OnChopFinish += DespawnRpc;
    }

    public override void OnNetworkDespawn()
    {
        foreach (var choppable in choppableObjects) choppable.OnChopFinish -= DespawnRpc;
    }

    [Rpc(SendTo.Server)]
    private void DespawnRpc(NetworkBehaviourReference choppable)
    {
        choppable.TryGet<NetworkChoppableObject>(out var choppedObject);
        choppedObject.NetworkObject.Despawn(false);
    }

    public void RespawnAllRpc()
    {
        foreach (var choppable in choppableObjects)
        {
            choppable.ResetDurability();
            choppable.transform.root.gameObject.SetActive(true);
        }
    }
}