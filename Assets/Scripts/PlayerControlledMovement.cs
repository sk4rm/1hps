using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControlledMovement : MonoBehaviour
{
    [SerializeField] private new Rigidbody rigidbody;
    [SerializeField] private new Camera camera;
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    private InputSystem_Actions _playerActions;

    private void Awake()
    {
        _playerActions = new InputSystem_Actions();
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

    private void Update()
    {
        var forward = camera.transform.forward;
        var right = camera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        
        var direction = _playerActions.Player.Move.ReadValue<Vector2>();
        
        var velocity = new Vector3(0, rigidbody.linearVelocity.y, 0);
        velocity += forward * (direction.y * speed);
        velocity += right * (direction.x * speed);
        rigidbody.linearVelocity = velocity;
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        var velocity = rigidbody.linearVelocity;
        velocity.y = jumpSpeed;
        rigidbody.linearVelocity = velocity;
    }
}