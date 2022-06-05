using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyUI : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button find;
    [SerializeField] private Button exit;
    [SerializeField] private Button ready;
    [SerializeField] private TextMeshProUGUI readyText;

    [SerializeField] private TextMeshProUGUI players;

    private int maxPlayers = 2;
    private bool readyState = false;
    private Lobby lobby;

    private void Awake()
    {
        lobby = GetComponent<Lobby>();

        find.onClick.AddListener(JoinOrCreateRoom);
        exit.onClick.AddListener(LeaveRoom);
        ready.onClick.AddListener(ReadyButton);
    }

    public override void OnJoinedRoom()
    {
        var hash = PhotonNetwork.LocalPlayer.CustomProperties;

        if (!hash.ContainsKey("Ready"))
        {
            hash.Add("Ready", false);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    private void ReadyButton()
    {
        readyState = !readyState;
        var hash = PhotonNetwork.LocalPlayer.CustomProperties;

        if (hash.ContainsKey("Ready"))
        {
            hash["Ready"] = readyState;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

            if (readyState)
                readyText.text = "Is Ready";
            else readyText.text = "Not Ready";
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < maxPlayers) return;
        bool allReady = true;

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            var hash = PhotonNetwork.PlayerList[i].CustomProperties;

            if (hash.ContainsKey("Ready"))
            {
                var value = (bool)hash["Ready"];
                if (!value) allReady = false;
            }
        }

        if (allReady && PhotonNetwork.IsMasterClient)
        {
            ready.interactable = false;
            lobby.StartMatch();
        }
    }

    private void JoinOrCreateRoom()
    {
        RoomOptions options = new RoomOptions() { MaxPlayers = 2 };
        PhotonNetwork.JoinRandomOrCreateRoom(null, 0, MatchmakingMode.FillRoom, null, null, null, options);
    }

    private void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void AllDisable()
    {
        find.interactable = false;
        exit.interactable = false;
        ready.interactable = false;
    }

    public void OutsideTheRoom()
    {
        find.interactable = true;
        exit.interactable = false;
        ready.interactable = false;
    }

    public void InTheRoom()
    {
        find.interactable = false;
        exit.interactable = true;
        ready.interactable = true;
    }

    public void AddPlayerInRoom()
    {
        players.text = "";

        foreach (var item in PhotonNetwork.CurrentRoom.Players)
        {
            players.text += item.Value.NickName + "\n";
        }
    }

    public void ClearPlayersInRoom()
    {
        players.text = "";
    }
}
