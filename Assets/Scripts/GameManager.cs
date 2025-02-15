using System;
using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Error;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

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

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
    }

    public void StartClient(string url = "127.0.0.1", ushort port = 7777)
    {
        var hostEntry = Dns.GetHostEntry(url);
        var ip = hostEntry.AddressList[0].ToString();
        
        // IPv6 loopback doesn't work for some reason.
        if (ip == "::1") ip = "127.0.0.1";
        
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ip, port);
        NetworkManager.Singleton.StartClient();
    }

    public void LeaveSession()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}