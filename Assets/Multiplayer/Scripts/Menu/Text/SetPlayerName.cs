using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetPlayerName : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Awake()
    {
        NetworkManager.Instance.OnChangePlayerName.AddListener(ChangeHandle);
    }

    private void Start() 
    {
        ChangeHandle(NetworkManager.Instance.PlayerName);
    }

    private void ChangeHandle(string newText)
    {
        text.text = newText;
    }
}
