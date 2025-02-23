using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class NetworkPlayerMovement : NetworkBehaviour
{
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private new Camera camera;
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float lookSpeed = 10f;
    [SerializeField] private ColliderEventDispatcher groundChecker;

    private Vector3 _lastDirection;
    private bool _canJump;

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

        var axis = PlayerControls.Instance.Actions.Player.Move.ReadValue<Vector2>();
        var direction = forward * axis.y + right * axis.x;

        if (PlayerControls.Instance.Actions.Player.Move.IsPressed())
            _lastDirection = direction;

        Move(direction);
        Look(_lastDirection);
    }

    private void OnEnable()
    {
        PlayerControls.Instance.Actions.Player.Jump.performed += Jump;
        groundChecker.OnTriggerEntered += OnGroundCheckerEnter;
        groundChecker.OnTriggerExited += OnGroundCheckerExit;
    }

    private void OnDisable()
    {
        PlayerControls.Instance.Actions.Player.Jump.performed -= Jump;
        groundChecker.OnTriggerEntered -= OnGroundCheckerEnter;
        groundChecker.OnTriggerExited -= OnGroundCheckerExit;
    }

    private void OnGroundCheckerEnter(Collider other)
    {
        _canJump = true;
    }

    private void OnGroundCheckerExit(Collider other)
    {
        _canJump = false;
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
        if (!_canJump) return;
        _canJump = false;
        
        print("jumped");

        var velocity = rigidbody.linearVelocity;
        velocity.y = jumpSpeed;
        rigidbody.linearVelocity = velocity;
    }

    private void Look(Vector3 direction)
    {
        var rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * lookSpeed);
    }
}