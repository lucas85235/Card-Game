using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BasicConection : MonoBehaviourPunCallbacks
{
    [Header("DEBUG")]
    [SerializeField] private int ping;
    [SerializeField] private bool isConnected = false;

    public bool IsConnected { get => isConnected; }

    public static BasicConection Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    private void FixedUpdate()
    {
        if (isConnected)
        {
            ping = PhotonNetwork.GetPing();
        }
        else PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        Debug.Log("OnConnected");
        isConnected = true;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError("OnJoinRandomFailed");

        var randName = "Room" + Random.Range(100, 1000);
        PhotonNetwork.CreateRoom(randName);

        Debug.Log("Creating a new room: " + randName);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("OnDisconnected: " + cause);
        isConnected = false;
    }
}
