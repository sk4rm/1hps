using Unity.Netcode;
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

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}