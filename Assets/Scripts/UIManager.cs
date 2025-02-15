using System;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public delegate void OnMessageSubmittedDelegate(string message);

    [Header("Chat UI")] [SerializeField] private RectTransform chatBox;
    [SerializeField] private TextMeshProUGUI chatBoxText;
    [SerializeField] private RectTransform chatBar;
    [SerializeField] private TMP_InputField chatBarText;
    [SerializeField] private float chatShowDurationSeconds = 3;
    private float _hideChatTimer;
    private bool _pauseHideChatTimer;

    [Header("Pause Menu")] [SerializeField]
    private RectTransform pauseMenu;

    [SerializeField] private CinemachineInputAxisController cinemachineInputAxisController;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        chatBarText.onFocusSelectAll = true;
    }

    private void Update()
    {
        if (!_pauseHideChatTimer)
        {
            _hideChatTimer = Math.Max(0, _hideChatTimer - Time.deltaTime);
            if (_hideChatTimer <= 0) chatBox.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        PlayerControls.Instance.Actions.UI.OpenChat.performed += OpenChatBox;
        PlayerControls.Instance.Actions.UI.Submit.performed += Submit;
        PlayerControls.Instance.Actions.UI.Cancel.performed += Cancel;
        NetworkChatSystem.OnReceive += OnChatMessageReceived;
        exitButton.onClick.AddListener(GameManager.Instance.ExitToMainMenu);
    }

    private void OnDisable()
    {
        PlayerControls.Instance.Actions.UI.OpenChat.performed -= OpenChatBox;
        PlayerControls.Instance.Actions.UI.Submit.performed -= Submit;
        PlayerControls.Instance.Actions.UI.Cancel.performed -= Cancel;
        NetworkChatSystem.OnReceive -= OnChatMessageReceived;
        exitButton.onClick.RemoveListener(GameManager.Instance.ExitToMainMenu);
    }

    private void OpenChatBox(InputAction.CallbackContext ctx)
    {
        PlayerControls.Instance.Actions.Player.Disable();
        chatBox.gameObject.SetActive(true);
        chatBar.gameObject.SetActive(true);
        _pauseHideChatTimer = true;
        chatBarText.Select();
    }

    private void FlashChatBox(float durationSeconds)
    {
        chatBox.gameObject.SetActive(true);
        _hideChatTimer = durationSeconds;
        _pauseHideChatTimer = false;
    }

    private void Submit(InputAction.CallbackContext ctx)
    {
        if (chatBarText.isFocused)
        {
            OnChatBarSubmit?.Invoke(chatBarText.text);
            chatBarText.text = "";
            chatBarText.Select();
        }
    }

    private void Cancel(InputAction.CallbackContext ctx)
    {
        if (chatBar.gameObject.activeSelf)
        {
            PlayerControls.Instance.Actions.Player.Enable();
            chatBar.gameObject.SetActive(false);
            FlashChatBox(chatShowDurationSeconds);
        }
        else if (pauseMenu.gameObject.activeSelf)
        {
            PlayerControls.Instance.Actions.Player.Enable();
            cinemachineInputAxisController.enabled = true;
            pauseMenu.gameObject.SetActive(false);
        }
        else
        {
            PlayerControls.Instance.Actions.Player.Disable();
            cinemachineInputAxisController.enabled = false;
            chatBar.gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(true);
        }
    }

    private void OnChatMessageReceived(string message)
    {
        chatBoxText.text += message;
        FlashChatBox(chatShowDurationSeconds);
    }

    public static event OnMessageSubmittedDelegate OnChatBarSubmit;
}