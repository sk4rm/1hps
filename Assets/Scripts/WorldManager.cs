using System;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private NetworkPlayerSpawner playerSpawner;
    [SerializeField] private Player player;
    [SerializeField] private UIManager ui;

    private Player Player
    {
        get => player;
        set => player = value;
    }

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
        Player = p;
        Player.Inventory.Wood.OnValueChanged += UpdateWoodCount;
    }

    private void UpdateWoodCount(int previousCount, int newCount)
    {
        ui.WoodCountText.text = newCount.ToString();
    }
}
