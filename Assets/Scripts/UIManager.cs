using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform chatBox;
    [SerializeField] private TextMeshProUGUI chatBoxBody;
    [SerializeField] private RectTransform chatBar;
    [SerializeField] private TMP_InputField chatBarInput;
    [SerializeField] private float chatShowDurationSeconds = 3;
    private float _hideChatTimer;
    private bool _pauseHideChatTimer;

    private void Awake()
    {
        chatBarInput.onFocusSelectAll = true;
    }
    
    private void OnEnable()
    {
        PlayerControls.Instance.Actions.UI.OpenChat.performed += OpenChatBox;
        PlayerControls.Instance.Actions.UI.Submit.performed += Submit;
        PlayerControls.Instance.Actions.UI.Cancel.performed += Cancel;
        ChatSystem.OnReceive += OnChatMessageReceived;
    }

    private void OnDisable()
    {
        PlayerControls.Instance.Actions.UI.OpenChat.performed -= OpenChatBox;
        PlayerControls.Instance.Actions.UI.Submit.performed -= Submit;
        PlayerControls.Instance.Actions.UI.Cancel.performed -= Cancel;
        ChatSystem.OnReceive -= OnChatMessageReceived;
    }

    private void OpenChatBox(InputAction.CallbackContext ctx)
    {
        chatBox.gameObject.SetActive(true);
        chatBar.gameObject.SetActive(true);
        _pauseHideChatTimer = true;
        chatBarInput.Select();
    }

    private void FlashChatBox(float durationSeconds)
    {
        chatBox.gameObject.SetActive(true);
        _hideChatTimer = durationSeconds;
        _pauseHideChatTimer = false;
    }

    private void Submit(InputAction.CallbackContext ctx)
    {
        if (chatBarInput.isFocused)
        {
            OnMessageSubmitted?.Invoke(chatBarInput.text);
            chatBarInput.text = "";
            chatBarInput.Select();
        }
    }

    private void Cancel(InputAction.CallbackContext ctx)
    {
        if (chatBar.gameObject.activeSelf)
        {
            chatBar.gameObject.SetActive(false);
            FlashChatBox(chatShowDurationSeconds);
        }
    }

    private void OnChatMessageReceived(string message)
    {
        chatBoxBody.text += message;
        FlashChatBox(chatShowDurationSeconds);
    }

    private void Update()
    {
        if (!_pauseHideChatTimer)
        {
            _hideChatTimer = Math.Max(0, _hideChatTimer - Time.deltaTime);
            if (_hideChatTimer <= 0) chatBox.gameObject.SetActive(false);
        }
    }

    public delegate void OnMessageSubmittedDelegate(string message);
    public static event OnMessageSubmittedDelegate OnMessageSubmitted;
}