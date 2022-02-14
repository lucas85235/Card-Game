using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class BasicConection : MonoBehaviourPunCallbacks
{
    [Header("ROBOTS")]
    [SerializeField] private List<Robot> robots;

    [Header("DEBUG")]
    [SerializeField] private int ping;
    [SerializeField] private bool isConnected = false;
    [SerializeField] private bool inRoon = false;

    public static BasicConection Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = "Player" + Random.Range(100, 1000);
    }

    private void FixedUpdate()
    {
        if (isConnected)
        {
            ping = PhotonNetwork.GetPing();
        }
        else PhotonNetwork.ConnectUsingSettings();
    }

    public bool IsReady()
    {
        if (!inRoon) return false;

        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        int maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;

        return playerCount == maxPlayers;
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

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom: " + PhotonNetwork.NickName);

        inRoon = true;

        var index = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        Robot player = robots[index];

        var tempPlayer = PhotonNetwork.Instantiate(
            player.name,
            player.transform.position,
            player.transform.rotation,
            0
        );

        if (PhotonNetwork.CurrentRoom.PlayerCount ==
            PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable() { { "Start", true } });
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        inRoon = false;
        Debug.LogError("OnJoinRandomFailed");
        CreateRoom();
    }

    public override void OnLeftRoom()
    {
        inRoon = false;
        Debug.LogError("OnLeftRoom");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("OnDisconnected: " + cause);
        isConnected = false;
    }

    private void CreateRoom()
    {
        var randName = "Room" + Random.Range(100, 1000);
        var options = new RoomOptions();
        options.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(randName, options);
        Debug.Log("Creating a new room: " + randName);
    }
}
