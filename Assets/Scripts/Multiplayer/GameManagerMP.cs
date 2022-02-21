using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManagerMP : MonoBehaviourPunCallbacks
{
    public static GameManagerMP Instance;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnJoinedRoom()
    {
        
    }
}
