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

            OnReceive?.Invoke("\n" + output);
            return;
        }

        SendChatRpc($"Player {NetworkManager.Singleton.LocalClient}: {message}");
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SendChatRpc(string message, RpcParams rpcParams = default)
    {
        OnReceive?.Invoke("\n" + message);
    }

    private bool TryExecuteSlashCommand(string command, out string output)
    {
        if (command.StartsWith("/respawntrees"))
        {
            ChoppableManager.Instance.RespawnAllRpc();
            output = $"<i>{NetworkManager.Singleton.LocalClientId} respawned all trees!</i>";
            return true;
        }

        output = "<i>Invalid command</i>";
        return false;
    }

    public static event OnReceiveDelegate OnReceive;
}