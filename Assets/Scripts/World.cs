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
        LocalPlayer.Inventory.Money.OnValueChanged += UpdateMoneyCount;
        print($"Bound player {LocalPlayer.OwnerClientId}");
        
        Cabin.Menu.SellWoodButton.onClick.AddListener(SellPlayerWood);
    }

    private void UpdateWoodCount(int previousCount, int newCount)
    {
        ui.WoodCountText.text = newCount.ToString();
    }

    private void SellPlayerWood()
    {
        LocalPlayer.Inventory.SellWoodRpc(Cabin.MoneyPerWood);
    }

    private void UpdateMoneyCount(int previousCount, int newCount)
    {
        ui.MoneyCountText.text = $"${newCount.ToString()}";
    }
}
