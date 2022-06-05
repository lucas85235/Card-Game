using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class CloseRoom : MonoBehaviourPunCallbacks
{
    [Space]
    [Header("On Leave Room")]
    public UnityEvent OnLeaveRoom;

    public void LoadLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (otherPlayer != PhotonNetwork.LocalPlayer)
        {
            PhotonNetwork.LeaveRoom();            
            Debug.Log("Other Player");
        }
    }

    public override void OnLeftRoom()
    {
        OnLeaveRoom?.Invoke();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        OnLeaveRoom?.Invoke();
    }
}
