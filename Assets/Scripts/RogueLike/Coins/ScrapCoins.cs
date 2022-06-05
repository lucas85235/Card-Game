using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScrapCoins : MonoBehaviour
{
    private int _coins;
    private string _coinsDataKey = "Coins";

    [Space]
    public UnityEvent OnUpdateCoins;

    public int TotalCoins
    {
        get => _coins;
        set
        {
            _coins = value;
            SaveData();
            OnUpdateCoins?.Invoke();
        }
    }

    public static ScrapCoins Instance;

    private void Awake()
    {
        Instance = this;
        LoadData();
    }

    public void AddCoins(int inc)
    {
        TotalCoins += inc;
    }

    [ContextMenu("Save Data")]
    public void SaveData()
    {
        PlayerPrefs.SetInt(_coinsDataKey, TotalCoins);
        PlayerPrefs.Save();
    }

    [ContextMenu("Load Data")]
    public void LoadData()
    {
        TotalCoins = PlayerPrefs.GetInt(_coinsDataKey);
    }
}
