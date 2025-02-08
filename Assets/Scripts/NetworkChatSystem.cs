using Unity.Netcode;
using UnityEngine;

public class NetworkChatSystem : NetworkBehaviour
{
    public delegate void OnReceiveDelegate(string message);

    private const string MessageOfTheDay = "<i>Press ENTER to chat</i>";

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
        OnReceive?.Invoke(MessageOfTheDay);
        Debug.Log("Chat system initialized!");
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
        OnReceive?.Invoke(entry);
    }

    public static event OnReceiveDelegate OnReceive;
}