using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public InputSystem_Actions Actions;
    public static PlayerInputManager Instance { get; private set; }

    private void Awake()
    {
        #region Singleton

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        #endregion

        Actions = new InputSystem_Actions();
        Debug.Log("Input system actions initialized!");
    }

    private void OnEnable()
    {
        Actions.Enable();
    }

    private void OnDisable()
    {
        Actions.Disable();
    }
}