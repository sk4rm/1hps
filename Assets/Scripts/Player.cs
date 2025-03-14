using System;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [field: SerializeField] public Chopper Chopper { get; private set; }
    [field: SerializeField] public Inventory Inventory { get; private set; }

    private void OnEnable()
    {
        Chopper.OnChopFinish += OnChoppableChopFinish;
    }

    private void OnDisable()
    {
        Chopper.OnChopFinish -= OnChoppableChopFinish;
    }

    private void OnChoppableChopFinish(ChoppableBehaviour choppable)
    {
        if (choppable.ResourceType == ItemType.Wood)
            Inventory.AddWoodRpc(choppable.ResourceAmount);
    }
}