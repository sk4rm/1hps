using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChopInteractor : NetworkBehaviour
{
    [SerializeField] private float chopEfficiency = 1f;
    [SerializeField] private float maxChopRange = 5f;
    
    private new Camera camera;
    
    public override void OnNetworkSpawn()
    {
        PlayerInputManager.Instance.Actions.Player.Attack.performed += Chop;
        camera = Camera.main;
    }

    public override void OnNetworkDespawn()
    {
        PlayerInputManager.Instance.Actions.Player.Attack.performed -= Chop;
    }
    
    private void Chop(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;
        ChopRpc();
    }

    [Rpc(SendTo.Server)]
    private void ChopRpc()
    {
        // FIXME chopRpc not working on remote client
        print("chop rpc called from chop interactor");
        
        Physics.Raycast(
            transform.position,
            camera.transform.forward,
            out var hit,
            maxChopRange
        );

        if (hit.collider == null) return;

        if (hit.collider.gameObject.TryGetComponent<Choppable>(out var choppable))
            choppable.ChopRpc(chopEfficiency);
    }
}