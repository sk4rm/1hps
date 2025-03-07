using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChopBehaviour : NetworkBehaviour
{
    [SerializeField] private float chopEfficiency = 1f;
    [SerializeField] private float maxChopRange = 5f;
    
    public override void OnNetworkSpawn()
    {
        PlayerInputManager.Instance.Actions.Player.Attack.performed += Chop;
    }

    public override void OnNetworkDespawn()
    {
        PlayerInputManager.Instance.Actions.Player.Attack.performed -= Chop;
    }
    
    private void Chop(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;
        ChopRpc(transform.forward);
    }

    [Rpc(SendTo.Server)]
    private void ChopRpc(Vector3 direction)
    {
        Physics.Raycast(
            transform.position,
            direction,
            out var hit,
            maxChopRange
        );

        if (hit.collider == null) return;

        if (hit.collider.gameObject.TryGetComponent<ChoppableBehaviour>(out var choppable))
            choppable.ChopRpc(chopEfficiency);
    }
}