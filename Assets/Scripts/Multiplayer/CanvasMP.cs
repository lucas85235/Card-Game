using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CanvasMP : MonoBehaviour
{
    [Header("Setup")]
    public GameObject playerCanvas;

    private void Start()
    {
        if (TryGetComponent(out PhotonView view))
        {
            playerCanvas.SetActive(view.IsMine);
        }

        PhotonNetwork.NickName = "Player" + Random.Range(100, 1000);
    }
}
