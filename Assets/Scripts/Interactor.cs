using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : NetworkBehaviour
{
    [SerializeField] private float maxReach = 10f;
    
    public override void OnNetworkSpawn()
    {
        PlayerInputManager.Instance.Actions.Player.Interact.performed += Interact;
    }

    public override void OnNetworkDespawn()
    {
        PlayerInputManager.Instance.Actions.Player.Interact.performed -= Interact;
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;
        
        Physics.Raycast(
            transform.position,
            transform.forward,
            out var hit,
            maxReach
        );
        
        if (hit.collider == null) return;

        if (hit.collider.gameObject.TryGetComponent<Interactable>(out var interactable))
        {
            interactable.Interact();
        }
    }
}