using Unity.Netcode;
using UnityEngine;

public class ChoppableManager : NetworkBehaviour
{
    public static ChoppableManager Instance;

    [SerializeField] private ChoppableBehaviour[] choppableObjects;

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

        choppableObjects = FindObjectsByType<ChoppableBehaviour>(FindObjectsSortMode.None);
    }

    [Rpc(SendTo.Server)]
    public void RespawnAllRpc()
    {
        foreach (var choppable in choppableObjects)
        {
            choppable.ResetDurability();
            choppable.transform.root.gameObject.SetActive(true);
            choppable.NetworkObject.Spawn();
        }
    }
}