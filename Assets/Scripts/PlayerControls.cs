using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public static PlayerControls instance;
    
    private InputSystem_Actions _actions;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        _actions = new InputSystem_Actions();
        Debug.Log("Input system actions initialized!");
    }

    private void OnEnable()
    {
        _actions.Enable();

        _actions.Player.Jump.performed += NotifyJump;
    }

    private void OnDisable()
    {
        _actions.Disable();
        
        _actions.Player.Jump.performed -= NotifyJump;
    }

    public Vector2 ReadMoveAxis()
    {
        return _actions.Player.Move.ReadValue<Vector2>();
    }

    public delegate void OnJumpDelegate(InputAction.CallbackContext ctx);
    public event OnJumpDelegate OnJump;

    private void NotifyJump(InputAction.CallbackContext ctx)
    {
        OnJump?.Invoke(ctx);
    }
}
