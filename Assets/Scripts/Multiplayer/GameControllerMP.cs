using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameControllerMP : MonoBehaviourPunCallbacks
{
    [Header("CHARACTERS")]
    public List<RobotMP> robots = new List<RobotMP>();
    private PhotonView view;

    public bool isReady = false;

    public static GameControllerMP Instance;

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        view = GetComponent<PhotonView>();

        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        yield return new WaitUntil(() => PhotonNetwork.PlayerList.Length == PhotonNetwork.CurrentRoom.MaxPlayers);
        yield return new WaitForSeconds(0.1f);

        Debug.Log("Sala cheia");

        if (PhotonNetwork.IsMasterClient)
        {
            view.RPC("SetRobots", RpcTarget.All);
        }
    }

    [PunRPC]
    private void SetRobots()
    {
        robots = new List<RobotMP>(FindObjectsOfType<RobotMP>());

        foreach (var item in robots)
        {
            Debug.Log("item -> " + item.name);
        }

        isReady = true;
    }
}
