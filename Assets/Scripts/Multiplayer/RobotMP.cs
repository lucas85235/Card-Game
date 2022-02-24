using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RobotMP : MonoBehaviour
{
    public Transform selectedCardsConteriner;

    public LifeMP life { get; private set; }

    private Hashtable CustomeValue;
    private PhotonView view;

    void Start()
    {
        life = GetComponent<LifeMP>();
        view = GetComponent<PhotonView>();
    }
}
