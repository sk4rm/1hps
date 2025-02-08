using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private new Camera camera;
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;

    private void Awake()
    {
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();
        if (camera == null) camera = Camera.main;
    }

    private void Update()
    {
        var forward = camera.transform.forward;
        var right = camera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        var axis = PlayerControls.instance.ReadMoveAxis();
        var direction = forward * axis.y + right * axis.x;

        Move(direction);
    }

    private void OnEnable()
    {
        PlayerControls.instance.OnJump += Jump;
    }

    private void OnDisable()
    {
        PlayerControls.instance.OnJump -= Jump;
    }

    private void Move(Vector3 direction)
    {
        if (!IsOwner) return;
        MoveRpc(direction);
    }

    [Rpc(SendTo.Server)]
    private void MoveRpc(Vector3 direction)
    {
        // Debug.Log($"MoveRpc invoked on network object #{NetworkObjectId} by {rpcParams.Receive.SenderClientId}");
        
        var velocity = new Vector3(0, rigidbody.linearVelocity.y, 0)
        {
            x = direction.x * speed,
            z = direction.z * speed
        };
        rigidbody.linearVelocity = velocity;
    }
    
    private void Jump(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;
        JumpRpc();
    }

    [Rpc(SendTo.Server)]
    private void JumpRpc()
    {
        var velocity = rigidbody.linearVelocity;
        velocity.y = jumpSpeed;
        rigidbody.linearVelocity = velocity;
    }
}