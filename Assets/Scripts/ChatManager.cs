using System;
using System.Text.RegularExpressions;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChatManager : NetworkBehaviour
{
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

    public static event Action<string> OnReceive;

    public override void OnNetworkSpawn()
    {
        OnReceive?.Invoke(messageOfTheDay);
        Debug.Log("Chat system initialized!");
    }

    private void SendChat(string message)
    {
        message = Regex.Replace(message, "<.*?>", string.Empty);

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

        SendChatRpc($"{GameManager.Instance.localPlayerDisplayName}: {message}");
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
            RespawnTreesRpc();
            output = $"<i>{GameManager.Instance.localPlayerDisplayName} respawned all trees!</i>";
            return true;
        }

        output = "<i>Invalid command</i>";
        return false;
    }

    [Rpc(SendTo.Server)]
    private void RespawnTreesRpc()
    {
        ChoppableBehaviour.RespawnChopped();
    }
}