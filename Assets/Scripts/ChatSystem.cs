using System;
using Unity.Netcode;
using UnityEngine;

public class ChatSystem : NetworkBehaviour
{
    [SerializeField] private string chat;
    private const string MessageOfTheDay = "<i>Press ENTER to chat</i>";

    public override void OnNetworkSpawn()
    {
        chat = MessageOfTheDay;
        OnReceive?.Invoke(MessageOfTheDay);
        
        Debug.Log($"Chat system initialized!");
    }

    private void OnEnable()
    {
        UIManager.OnMessageSubmitted += SendChat;
    }

    private void OnDisable()
    {
        UIManager.OnMessageSubmitted -= SendChat;
    }

    private void SendChat(string message)
    {
        SendChatRpc(message);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SendChatRpc(string message, RpcParams rpcParams = default)
    {
        var senderId = rpcParams.Receive.SenderClientId;
        var entry = $"\nPlayer {senderId}: {message}";
        
        chat += entry;
        OnReceive?.Invoke(entry);
    }

    public delegate void OnReceiveDelegate(string message);
    public static event OnReceiveDelegate OnReceive;
}
