using System;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class World : NetworkBehaviour
{
    [SerializeField] private NetworkPlayerSpawner playerSpawner;
    [SerializeField] private UI ui;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [field: SerializeField] public Shop.Shop Cabin { get; private set; }
    
    [Header("Runtime Fields")]
    [field: SerializeField] public Player LocalPlayer { get; private set; }

    private void Awake()
    {
        ui.CinemachineCamera = cinemachineCamera;
    }

    private void OnEnable()
    {
        playerSpawner.OnPlayerSpawn += BindPlayer;
        Cabin.Menu.OnOpen += OnCabinMenuOpen;
        Cabin.Menu.OnClose += OnCabinMenuClose;
    }

    private void OnDisable()
    {
        playerSpawner.OnPlayerSpawn -= BindPlayer;
        Cabin.Menu.OnOpen -= OnCabinMenuOpen;
        Cabin.Menu.OnClose -= OnCabinMenuClose;
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
    
    private void OnCabinMenuOpen()
    {
        cinemachineCamera.GetComponent<CinemachineInputAxisController>().enabled = false;
    }
    
    private void OnCabinMenuClose()
    {
        cinemachineCamera.GetComponent<CinemachineInputAxisController>().enabled = true;
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
