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
    [SerializeField] private bool inRoon = false;

    public bool IsReady()
    { 
        if (!inRoon) return false;

        int temp = PhotonNetwork.CurrentRoom.PlayerCount;
        return temp > 1;
    }

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

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom: " + PhotonNetwork.CurrentRoom.Name);

        Robot player;
        
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            player = GameController.i.playerOne;
        else player = GameController.i.playerTwo;

        var tempPlayer = PhotonNetwork.Instantiate(
            player.name, 
            player.transform.position, 
            player.transform.rotation,
            0 
        );

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            GameController.i.playerOne = tempPlayer.GetComponent<Robot>();
        else GameController.i.playerTwo = tempPlayer.GetComponent<Robot>();

        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
            inRoon = true;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        inRoon = false;
        Debug.LogError("OnJoinRandomFailed");

        var randName = "Room" + Random.Range(100, 1000);
        PhotonNetwork.CreateRoom(randName);

        Debug.Log("Creating a new room: " + randName);
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
}
