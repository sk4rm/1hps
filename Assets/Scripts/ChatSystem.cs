using System;
using TMPro;
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
    }

    private void Awake()
    {
        UIManager.OnMessageSubmitted += SendChat;
    }

    private void SendChat(string message)
    {
        //TODO: RPC to send message and synchronize with clients
    }

    public delegate void OnReceiveDelegate(string message);
    public static event OnReceiveDelegate OnReceive;
}
