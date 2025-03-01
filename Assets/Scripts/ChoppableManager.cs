using Unity.Netcode;
using UnityEngine;

public class ChoppableManager : NetworkBehaviour
{
    public static ChoppableManager Instance;

    private NetworkChoppableObject[] choppableObjects;

    private void Awake()
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
    }

    private void OnEnable()
    {
        foreach (var choppable in choppableObjects) choppable.OnChopFinish += DespawnRpc;
    }

    private void OnDisable()
    {
        foreach (var choppable in choppableObjects) choppable.OnChopFinish -= DespawnRpc;
    }

    private void DespawnRpc(NetworkChoppableObject choppedObject)
    {
        NetworkObject.Despawn(false);
        choppedObject.transform.root.gameObject.SetActive(false);
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