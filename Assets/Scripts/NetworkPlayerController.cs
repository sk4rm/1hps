using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class NetworkPlayerController : NetworkBehaviour
{
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private new Camera camera;
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float maximumReachDistance = 5f;

    private bool _isOnGround;
    private Vector3 _lastDirection;

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

        if (PlayerInputManager.Instance.Actions.Player.Move.IsPressed())
            _lastDirection = direction;

        Move(direction);
        Look(_lastDirection);
    }

    private void OnEnable()
    {
        PlayerInputManager.Instance.Actions.Player.Jump.performed += Jump;
        PlayerInputManager.Instance.Actions.Player.Attack.performed += Attack;
    }

    private void OnDisable()
    {
        PlayerInputManager.Instance.Actions.Player.Jump.performed -= Jump;
        PlayerInputManager.Instance.Actions.Player.Attack.performed -= Attack;
    }

    private void OnCollisionStay(Collision other)
    {
        _isOnGround = true;
    }

    private void OnCollisionExit(Collision other)
    {
        _isOnGround = false;
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
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;
        if (!_isOnGround) return;

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

    private void Attack(InputAction.CallbackContext ctx)
    {
        if (!IsOwner) return;
        
        Physics.Raycast(
            transform.position, 
            camera.transform.forward,
            out RaycastHit hit, 
            maximumReachDistance
        );
        
        if (hit.collider == null) return;
        Debug.Log($"hit {hit.transform.gameObject.name}");
    }
}