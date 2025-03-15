using System;
using Unity.Netcode;
using UnityEngine;

public class World : NetworkBehaviour
{
    [SerializeField] private NetworkPlayerSpawner playerSpawner;
    [SerializeField] private UI ui;
    [field: SerializeField] public Player LocalPlayer { get; private set; }
    [field: SerializeField] public Shop.Shop Cabin { get; private set; }

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
        
        LocalPlayer = player;
        LocalPlayer.Inventory.Wood.OnValueChanged += UpdateWoodCount;
        print($"Bound player {LocalPlayer.OwnerClientId}");
    }

    private void UpdateWoodCount(int previousCount, int newCount)
    {
        ui.WoodCountText.text = newCount.ToString();
    }
}
