using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform chatBox;
    [SerializeField] private RectTransform chatBar;
    [SerializeField] private TMP_InputField chatInput;
    [SerializeField] private float chatShowDurationSeconds = 3;
    private float _hideChatTimer;
    private bool _pauseHideChatTimer;
    
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
        PlayerControls.Instance.Actions.UI.Cancel.performed -= Cancel;
        ChatSystem.OnReceive -= OnChatMessageReceived;
    }

    private void OpenChatBox(InputAction.CallbackContext ctx)
    {
        chatBox.gameObject.SetActive(true);
        chatBar.gameObject.SetActive(true);
        _pauseHideChatTimer = true;
    }

    private void FlashChatBox(float durationSeconds)
    {
        chatBox.gameObject.SetActive(true);
        _hideChatTimer = durationSeconds;
        _pauseHideChatTimer = false;
    }

    private void Submit(InputAction.CallbackContext ctx)
    {
        if (chatInput.isFocused)
        {
            OnMessageSubmitted?.Invoke(chatInput.text);
            chatInput.text = "";
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