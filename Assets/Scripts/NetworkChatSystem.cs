using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkChatSystem : NetworkBehaviour
{
    public delegate void OnReceiveDelegate(string message);

    private string messageOfTheDay;

    private void Awake()
    {
        var openChatKey = PlayerInputManager.Instance.Actions.UI.OpenChat.GetBindingDisplayString();
        messageOfTheDay = $"Press {openChatKey} to chat";
    }

    private void OnEnable()
    {
        UIManager.OnChatBarSubmit += SendChat;
    }

    private void OnDisable()
    {
        UIManager.OnChatBarSubmit -= SendChat;
    }

    public override void OnNetworkSpawn()
    {
        OnReceive?.Invoke(messageOfTheDay);
        Debug.Log("Chat system initialized!");
    }

    private void SendChat(string message)
    {
        if (message.StartsWith("/"))
        {
            if (TryExecuteSlashCommand(message, out var output))
            {
                SendChatRpc(output);
                return;
            }
            
            OnReceive?.Invoke(output);
            return;
        }
        
        SendChatRpc(message);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SendChatRpc(string message, RpcParams rpcParams = default)
    {
        var senderId = rpcParams.Receive.SenderClientId;
        var entry = $"\nPlayer {senderId}: {message}";
        OnReceive?.Invoke(entry);
    }

    private bool TryExecuteSlashCommand(string command, out string output)
    {
        if (command.StartsWith("/respawntrees"))
        {
            ChoppableManager.Instance.RespawnAll();
            output = $"<i>All trees respawned!</i>";
            return true;
        }

        output = "<i>Invalid command</i>";
        return false;
    }

    public static event OnReceiveDelegate OnReceive;
}