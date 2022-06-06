using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class EndTurn : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    [SerializeField] private Button endButton;
    [SerializeField] private bool testMode = false;

    private void Start()
    {
        endButton.onClick.AddListener(EndTurnButton);
    }

    private void EndTurnButton()
    {
        if (testMode)
        {
            Multiplayer.GameManager.Instance.EndRound();
            return;
        }

        var hash = PhotonNetwork.LocalPlayer.CustomProperties;

        if (hash.ContainsKey("EndTurn"))
        {
            if ((bool) hash["EndTurn"]) return;

            hash["EndTurn"] = true;
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

            endButton.interactable = false;
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("EndTurn"))
        {
            var value = (bool) changedProps["EndTurn"];

            if (!value)
            {
                endButton.interactable = true;
            }
        }
    }
}
