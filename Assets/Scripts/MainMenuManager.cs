using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button hostButton;
    public Button joinButton;
    public Button settingsButton;
    public Button exitButton;
    public static MainMenuManager Instance { get; private set; }

    private void Awake()
    {
        #region Singleton

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        #endregion
    }

    private void OnEnable()
    {
        hostButton.onClick.AddListener(GameManager.Instance.StartHost);
        joinButton.onClick.AddListener(GameManager.Instance.StartClient);
        exitButton.onClick.AddListener(GameManager.Instance.QuitGame);
    }

    private void OnDisable()
    {
        hostButton.onClick.RemoveListener(GameManager.Instance.StartHost);
        joinButton.onClick.RemoveListener(GameManager.Instance.StartClient);
        exitButton.onClick.RemoveListener(GameManager.Instance.QuitGame);
    }
}