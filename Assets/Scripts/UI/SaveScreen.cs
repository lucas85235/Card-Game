using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveScreen : MonoBehaviour
{
    [Space]
    [Header("Events")]
    public UnityEvent OnOpenGame;
    
    private string saveKey = "Open-Game";

    private void Awake()
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            OnOpenGame?.Invoke();
            Debug.Log("Event");
        }

        PlayerPrefs.SetInt(saveKey, 0);
    }
}
