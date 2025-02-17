using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NetworkManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
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
    }

    private void Start()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    private void OnEnable()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
    }

    private void OnDisable()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
    }

    private void OnClientDisconnect(ulong _)
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
    }

    public void StartClient(string hostname = "127.0.0.1", ushort port = 7777)
    {
        var hostEntry = Dns.GetHostEntry(hostname);
        var ip = hostEntry.AddressList[0].ToString();

        // IPv6 loopback doesn't work for some reason.
        if (ip == "::1") ip = "127.0.0.1";

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ip, port);
        NetworkManager.Singleton.StartClient();
    }

    public void ExitToMainMenu()
    {
        NetworkManager.Singleton.Shutdown();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}