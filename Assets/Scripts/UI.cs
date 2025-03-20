using System;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public CinemachineCamera CinemachineCamera { get; set; }

    private CinemachineInputAxisController CinemachineInputAxisController =>
        CinemachineCamera.GetComponent<CinemachineInputAxisController>();

    [Header("Chat UI")] [SerializeField] private RectTransform chatBox;
    [SerializeField] private TextMeshProUGUI chatBoxText;
    [SerializeField] private RectTransform chatBar;
    [SerializeField] private TMP_InputField chatBarInputField;
    [SerializeField] private float chatShowDurationSeconds = 3;

    [Header("Pause Menu")] [SerializeField]
    private RectTransform pauseMenu;

    [SerializeField] private Button exitButton;
    private float chatBoxTimer;

    [Header("Counters")] [SerializeField] private RectTransform woodCounter;
    [field: SerializeField] public TMP_Text WoodCountText { get; private set; }
    [SerializeField] private RectTransform moneyCounter;
    [field: SerializeField] public TMP_Text MoneyCountText { get; private set; }

    private bool IsChatOpen => chatBox.gameObject.activeSelf && chatBar.gameObject.activeSelf;

    private void Awake()
    {
        chatBarInputField.onFocusSelectAll = true;
        EnablePlayerControls();
    }

    private void Update()
    {
        if (!chatBar.gameObject.activeSelf)
        {
            chatBoxTimer = Math.Max(0, chatBoxTimer - Time.deltaTime);
            if (chatBoxTimer <= 0) chatBox.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        PlayerInputManager.Instance.Actions.UI.OpenChat.performed += OnOpenChat;
        PlayerInputManager.Instance.Actions.UI.Submit.performed += OnSubmit;
        PlayerInputManager.Instance.Actions.UI.Cancel.performed += OnCancel;
        ChatManager.OnReceive += OnChatMessageReceived;
        exitButton.onClick.AddListener(GameManager.Instance.ExitToMainMenu);
        woodCounter?.gameObject.SetActive(true);
        moneyCounter?.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        PlayerInputManager.Instance.Actions.UI.OpenChat.performed -= OnOpenChat;
        PlayerInputManager.Instance.Actions.UI.Submit.performed -= OnSubmit;
        PlayerInputManager.Instance.Actions.UI.Cancel.performed -= OnCancel;
        ChatManager.OnReceive -= OnChatMessageReceived;
        exitButton.onClick.RemoveListener(GameManager.Instance.ExitToMainMenu);
        if (woodCounter != null) woodCounter.gameObject.SetActive(false);
        if (moneyCounter != null) moneyCounter.gameObject.SetActive(false);
    }

    public static event Action<string> OnChatBarSubmit;

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
        PlayerInputManager.Instance.Actions.Player.Enable();
        CinemachineInputAxisController.enabled = true;
        GameManager.Instance.LockCursor();
    }

    private void DisablePlayerControls()
    {
        PlayerInputManager.Instance.Actions.Player.Disable();
        CinemachineInputAxisController.enabled = false;
        GameManager.Instance.UnlockCursor();
    }

    private void CloseChatBar()
    {
        chatBar.gameObject.SetActive(false);

        EnablePlayerControls();
    }

    private void ResetChatBoxTimer()
    {
        chatBoxTimer = chatShowDurationSeconds;
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
}