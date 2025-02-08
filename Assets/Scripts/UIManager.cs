using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform chatBox;
    [SerializeField] private RectTransform chatInput;
    
    private void OnEnable()
    {
        PlayerControls.Instance.Actions.UI.OpenChat.performed += OpenChatBox;
        PlayerControls.Instance.Actions.UI.Cancel.performed += Cancel;

        Debug.Log("Enabled UI Manager!");
    }

    private void OnDisable()
    {
        PlayerControls.Instance.Actions.UI.OpenChat.performed -= OpenChatBox;
        PlayerControls.Instance.Actions.UI.Cancel.performed += Cancel;

        Debug.Log("Disabled UI Manager!");
    }

    private void OpenChatBox(InputAction.CallbackContext ctx)
    {
        Debug.Log("Open Chat Box!");
        
        //TODO: auto close chat box after N seconds
        chatBox.gameObject.SetActive(true);
        chatInput.gameObject.SetActive(true);
    }

    private void Cancel(InputAction.CallbackContext ctx)
    {
        Debug.Log("Cancel!");
        
        if (chatInput.gameObject.activeSelf) chatInput.gameObject.SetActive(false);
    }
}