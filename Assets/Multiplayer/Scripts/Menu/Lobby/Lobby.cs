using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Lobby : MonoBehaviourPunCallbacks
{
    private LobbyUI ui;
    private PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        ui = GetComponent<LobbyUI>();
        ui.AllDisable();

        if (!PhotonNetwork.IsConnected)
        {
            // CONNECT TO SERVER
            PhotonNetwork.ConnectUsingSettings();
        }
        else OnConnectedToMaster();
    }

    public void ExitMultiplayer()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Join Lobby");
        ui.OutsideTheRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Join Room");

        ui.InTheRoom();
        ui.AddPlayerInRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Exit Room");

        ui.OutsideTheRoom();
        ui.ClearPlayersInRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ui.AddPlayerInRoom();
    }

    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        ui.AddPlayerInRoom();
    }

    public void StartMatch()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            var hash = PhotonNetwork.PlayerList[i].CustomProperties;

            if (hash.ContainsKey("Ready"))
            {
                hash["Ready"] = false;
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Gameplay");
        }
    }

    //        
    /// Events to implement later
    //

    public override void OnConnectedToMaster()
    {
        Debug.Log("On Connected");

        ui.OutsideTheRoom();
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("On Disconnected " + cause);
        ui.AllDisable();

        // Try Connect
        Invoke(nameof(PhotonNetwork.ConnectUsingSettings), 2f);
    }
}
