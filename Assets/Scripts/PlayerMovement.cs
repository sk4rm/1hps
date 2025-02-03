using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private new Camera camera;
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    private InputSystem_Actions _playerActions;

    private void Awake()
    {
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();
        if (camera == null) camera = Camera.main;

        _playerActions = new InputSystem_Actions();
    }

    private void Update()
    {
        var forward = camera.transform.forward;
        var right = camera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        var axis = _playerActions.Player.Move.ReadValue<Vector2>();
        var direction = forward * axis.y + right * axis.x;

        Move(direction);
    }

    private void OnEnable()
    {
        _playerActions.Enable();

        _playerActions.Player.Jump.performed += Jump;
    }

    private void OnDisable()
    {
        _playerActions.Disable();

        _playerActions.Player.Jump.performed -= Jump;
    }

    private void Move(Vector3 direction)
    {
        MoveRpc(direction);
    }

    [Rpc(SendTo.Server)]
    private void MoveRpc(Vector3 direction, RpcParams rpcParams = default)
    {
        var velocity = new Vector3(0, rigidbody.linearVelocity.y, 0)
        {
            x = direction.x * speed,
            z = direction.z * speed
        };
        rigidbody.linearVelocity = velocity;
    }
    
    private void Jump(InputAction.CallbackContext ctx)
    {
        JumpRpc();
    }

    [Rpc(SendTo.Server)]
    private void JumpRpc(RpcParams rpcParams = default)
    {
        var velocity = rigidbody.linearVelocity;
        velocity.y = jumpSpeed;
        rigidbody.linearVelocity = velocity;
    }
}