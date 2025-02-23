using System;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public delegate void OnMessageSubmittedDelegate(string message);

    [Header("Chat UI")] [SerializeField] private RectTransform chatBox;
    [SerializeField] private TextMeshProUGUI chatBoxText;
    [SerializeField] private RectTransform chatBar;
    [SerializeField] private TMP_InputField chatBarInputField;
    [SerializeField] private float chatShowDurationSeconds = 3;

    [Header("Pause Menu")] [SerializeField]
    private RectTransform pauseMenu;

    [SerializeField] private CinemachineInputAxisController cinemachineInputAxisController;
    [SerializeField] private Button exitButton;
    private float _chatBoxTimer;
    [Obsolete] private bool _pauseHideChatTimer;

    private bool IsChatOpen => chatBox.gameObject.activeSelf && chatBar.gameObject.activeSelf;

    private void Awake()
    {
        chatBarInputField.onFocusSelectAll = true;
        EnablePlayerControls();

        if (cinemachineInputAxisController == null)
            cinemachineInputAxisController = Camera.main!.GetComponent<CinemachineInputAxisController>();
    }

    private void Update()
    {
        if (!chatBar.gameObject.activeSelf)
        {
            _chatBoxTimer = Math.Max(0, _chatBoxTimer - Time.deltaTime);
            if (_chatBoxTimer <= 0) chatBox.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        PlayerControls.Instance.Actions.UI.OpenChat.performed += OnOpenChat;
        PlayerControls.Instance.Actions.UI.Submit.performed += OnSubmit;
        PlayerControls.Instance.Actions.UI.Cancel.performed += OnCancel;
        NetworkChatSystem.OnReceive += OnChatMessageReceived;
        exitButton.onClick.AddListener(GameManager.Instance.ExitToMainMenu);
    }

    private void OnDisable()
    {
        PlayerControls.Instance.Actions.UI.OpenChat.performed -= OnOpenChat;
        PlayerControls.Instance.Actions.UI.Submit.performed -= OnSubmit;
        PlayerControls.Instance.Actions.UI.Cancel.performed -= OnCancel;
        NetworkChatSystem.OnReceive -= OnChatMessageReceived;
        exitButton.onClick.RemoveListener(GameManager.Instance.ExitToMainMenu);
    }

    private void OnOpenChat(InputAction.CallbackContext ctx)
    {
        if (IsChatOpen) return;

        chatBox.gameObject.SetActive(true);
        ResetChatBoxTimer();

        OpenChatBar();
    }

    private void OpenChatBar()
    {
        chatBar.gameObject.SetActive(true);
        chatBarInputField.Select();

        DisablePlayerControls();
    }

    private void EnablePlayerControls()
    {
        PlayerControls.Instance.Actions.Player.Enable();
        cinemachineInputAxisController.enabled = true;
        GameManager.Instance.LockCursor();
    }

    private void DisablePlayerControls()
    {
        PlayerControls.Instance.Actions.Player.Disable();
        cinemachineInputAxisController.enabled = false;
        GameManager.Instance.UnlockCursor();
    }

    private void CloseChatBar()
    {
        chatBar.gameObject.SetActive(false);

        EnablePlayerControls();
    }

    private void ResetChatBoxTimer()
    {
        _chatBoxTimer = chatShowDurationSeconds;
    }

    private void OnSubmit(InputAction.CallbackContext ctx)
    {
        if (chatBarInputField.isFocused)
        {
            if (string.IsNullOrWhiteSpace(chatBarInputField.text)) return;

            OnChatBarSubmit?.Invoke(chatBarInputField.text);
            chatBarInputField.text = "";
        }
    }

    private void OpenPauseMenu()
    {
        pauseMenu.gameObject.SetActive(true);

        DisablePlayerControls();
    }

    private void ClosePauseMenu()
    {
        pauseMenu.gameObject.SetActive(false);

        EnablePlayerControls();
    }

    private void OnCancel(InputAction.CallbackContext ctx)
    {
        if (pauseMenu.gameObject.activeSelf)
        {
            ClosePauseMenu();
            return;
        }

        if (chatBar.gameObject.activeSelf)
        {
            CloseChatBar();
            return;
        }

        OpenPauseMenu();
    }

    private void OnChatMessageReceived(string message)
    {
        chatBoxText.text += message;

        chatBox.gameObject.SetActive(true);
        ResetChatBoxTimer();
    }

    public static event OnMessageSubmittedDelegate OnChatBarSubmit;
}