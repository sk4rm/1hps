using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button hostButton;
    public Button joinButton;
    public Button settingsButton;
    public Button exitButton;

    private bool _isConnecting;

    private void OnEnable()
    {
        hostButton.onClick.AddListener(GameManager.Instance.StartHost);
        joinButton.onClick.AddListener(GameManager.Instance.StartClient);
        exitButton.onClick.AddListener(GameManager.Instance.QuitGame);

        NetworkManager.Singleton.OnClientDisconnectCallback += OnConnectionFailed;
    }

    private void OnDisable()
    {
        hostButton.onClick.RemoveListener(GameManager.Instance.StartHost);
        joinButton.onClick.RemoveListener(GameManager.Instance.StartClient);
        exitButton.onClick.RemoveListener(GameManager.Instance.QuitGame);
        
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnConnectionFailed;
    }

    private void OnConnectionFailed(ulong _)
    {
        Debug.Log("Failed to connect to the server.");
    }
}