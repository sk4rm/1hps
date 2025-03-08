using UnityEngine;

[RequireComponent(typeof(Inventory), typeof(ChopBehaviour))]
public class Player : MonoBehaviour
{
    private ChopBehaviour chopper;
    private Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        chopper = GetComponent<ChopBehaviour>();
    }

    private void OnEnable()
    {
        chopper.OnChopFinish += OnChoppableChopFinish;
    }

    private void OnDisable()
    {
        chopper.OnChopFinish -= OnChoppableChopFinish;
    }

    private void OnChoppableChopFinish(ChoppableBehaviour choppable)
    {
        if (choppable.ResourceType == ItemType.Wood)
            inventory.AddWoodRpc(choppable.ResourceAmount);
    }
}