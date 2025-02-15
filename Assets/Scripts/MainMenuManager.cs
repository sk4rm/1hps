using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu Interface")]
    
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;
    
    [Header("Join Menu Interface")]
    
    [SerializeField] private RectTransform joinMenu;
    [SerializeField] private Button backToMainMenuButton;
    [SerializeField] private TMP_InputField ipInputField;
    [SerializeField] private TMP_InputField portInputField;
    [SerializeField] private Button joinMenuSubmitButton;
    [SerializeField] private TMP_Text errorText;

    private bool _isConnecting;

    private void OnEnable()
    {
        hostButton.onClick.AddListener(GameManager.Instance.StartHost);
        joinButton.onClick.AddListener(ShowJoinMenu);
        exitButton.onClick.AddListener(GameManager.Instance.QuitGame);
        
        backToMainMenuButton.onClick.AddListener(HideJoinMenu);
        joinMenuSubmitButton.onClick.AddListener(SubmitJoinRequest);

        NetworkManager.Singleton.OnClientDisconnectCallback += OnConnectionFailed;
    }

    private void OnDisable()
    {
        hostButton.onClick.RemoveListener(GameManager.Instance.StartHost);
        joinButton.onClick.AddListener(ShowJoinMenu);
        exitButton.onClick.RemoveListener(GameManager.Instance.QuitGame);

        backToMainMenuButton.onClick.RemoveListener(HideJoinMenu);
        joinMenuSubmitButton.onClick.RemoveListener(SubmitJoinRequest);
        
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnConnectionFailed;
    }

    private void OnConnectionFailed(ulong _)
    {
        errorText.text = $"Connection timed out!";
    }

    private void ShowJoinMenu()
    {
        joinMenu.gameObject.SetActive(true);
    }

    private void HideJoinMenu()
    {
        joinMenu.gameObject.SetActive(false);
    }

    private void SubmitJoinRequest()
    {
        var ok = ushort.TryParse(portInputField.text, out var port);
        if (!ok)
        {
            errorText.text = $"Invalid port number!";
            return;
        }

        try
        {
            GameManager.Instance.StartClient(ipInputField.text, port);
        }
        catch (Exception e)
        {
            errorText.text = e.Message;
        }
    }
}