using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Login : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private Button button;

    private void OnEnable()
    {
        if (NetworkManager.Instance.PlayerName != "")
        {
            button.onClick.Invoke();
            return;
        }
            
        var tempPlayer = "Player" + Random.Range(1000,10000);
        
        button.onClick.AddListener(() => 
        {
            if (input.text == "")
            {
                NetworkManager.Instance.PlayerName = tempPlayer;
            }
            else NetworkManager.Instance.PlayerName = input.text;
        });
    }
}
