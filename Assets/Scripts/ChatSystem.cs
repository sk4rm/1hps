using Unity.Netcode;
using UnityEngine;

public class ChatSystem : NetworkBehaviour
{
    [SerializeField] private string chat;
    private const string MessageOfTheDay = "<i>Press ENTER to chat</i>";

    public override void OnNetworkSpawn()
    {
        chat = MessageOfTheDay;
        OnReceive?.Invoke(chat);
    }

    private void Awake()
    {
        UIManager.OnMessageSubmitted += SendChat;
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
        Debug.Log(chat);
        OnReceive?.Invoke(entry);
    }

    public delegate void OnReceiveDelegate(string chat);
    public static event OnReceiveDelegate OnReceive;
}
