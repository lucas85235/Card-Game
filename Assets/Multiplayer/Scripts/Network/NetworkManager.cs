using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class NetworkManager : MonoBehaviour
{
    [HideInInspector] public UnityEvent<string> OnChangePlayerName;

    private string saveKey = "PLAYER-NAME";

    private string playerName = "";
    public string PlayerName
    {
        get => playerName;
        set
        {
            playerName = value;
            PlayerPrefs.SetString(saveKey, value);
            OnChangePlayerName?.Invoke(value);
            
            PhotonNetwork.NickName = value;
        }
    }

    #region Singleton

    public static NetworkManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

        if (PlayerPrefs.HasKey(saveKey))
        {
            PlayerName = PlayerPrefs.GetString(saveKey);
        }

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void OnApplicationQuit() 
    {
        PlayerPrefs.Save();    
    }

    #endregion
}
