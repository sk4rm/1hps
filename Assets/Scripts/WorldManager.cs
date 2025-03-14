using System;
using Unity.Netcode;
using UnityEngine;

public class WorldManager : NetworkBehaviour
{
    [SerializeField] private NetworkPlayerSpawner playerSpawner;
    [SerializeField] private UIManager ui;
    [field: SerializeField] public Player Player { get; private set; }

    private void OnEnable()
    {
        playerSpawner.OnPlayerSpawn += BindPlayer;
    }

    private void OnDisable()
    {
        playerSpawner.OnPlayerSpawn -= BindPlayer;
    }
    
    private void BindPlayer(Player p)
    {
        BindPlayerRpc(p);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void BindPlayerRpc(NetworkBehaviourReference playerObject)
    {
        playerObject.TryGet<Player>(out var player);
        if (!player.IsOwner) return;
        
        Player = player;
        Player.Inventory.Wood.OnValueChanged += UpdateWoodCount;
        print($"Bound player {Player.OwnerClientId}");
    }

    private void UpdateWoodCount(int previousCount, int newCount)
    {
        ui.WoodCountText.text = newCount.ToString();
    }
}
