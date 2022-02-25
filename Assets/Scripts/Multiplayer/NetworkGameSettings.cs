using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkGameSettings : MonoBehaviourPunCallbacks
{
    private void Start() 
    {
        
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }

}
