using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public InputSystem_Actions Actions;
    public static PlayerControls Instance { get; private set; }

    private void Awake()
    {
        #region Singleton

        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);

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