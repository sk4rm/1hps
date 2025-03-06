using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : NetworkBehaviour
{
    [Header("Movement")] [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private new Camera camera;
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private float turnSpeed = 10f;

    private bool isOnGround;
    private Vector3 lastDirection;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
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

        var axis = PlayerInputManager.Instance.Actions.Player.Move.ReadValue<Vector2>();
        var direction = forward * axis.y + right * axis.x;
        Move(direction);

        Look(lastDirection);
    }

    public override void OnNetworkSpawn()
    {
        PlayerInputManager.Instance.Actions.Player.Jump.performed += Jump;
    }

    public override void OnNetworkDespawn()
    {
        PlayerInputManager.Instance.Actions.Player.Jump.performed -= Jump;
    }

    private void OnCollisionExit()
    {
        isOnGround = false;
    }

    private void OnCollisionStay()
    {
        isOnGround = true;
    }

    private void Move(Vector3 direction)
    {
        if (!IsOwner) return;

        var velocity = new Vector3(0, rigidbody.linearVelocity.y, 0)
        {
            x = direction.x * speed,
            z = direction.z * speed
        };
        rigidbody.linearVelocity = velocity;

        
        if (direction.magnitude != 0) lastDirection = direction;
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;
        if (!isOnGround) return;

        var jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
        var velocity = rigidbody.linearVelocity;
        velocity.y = jumpSpeed;
        rigidbody.linearVelocity = velocity;
    }

    private void Look(Vector3 direction)
    {
        if (!IsOwner) return;

        var rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
    }
}