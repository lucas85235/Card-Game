using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RogueItems : MonoBehaviour
{
    [Space]
    [Header("Upgrades")]
    public UnityEvent OnUpdateAttack;
    public UnityEvent OnUpdateDefense;
    public UnityEvent OnUpdateIntelligence;
    public UnityEvent OnUpdateVelocity;

    [Space]
    [Header("Consumables")]
    public UnityEvent OnUpdateRepairs;
    public UnityEvent OnUpdateStickers;
    public UnityEvent OnUpdateSmokes;

    private Dictionary<string, int> upgrades = new Dictionary<string, int>();

    public int Attack
    {
        get => upgrades["attack"];
        set
        {
            SaveValue("attack", value);
            OnUpdateAttack?.Invoke();
        }
    }

    public int Defense
    {
        get => upgrades["defense"];
        set
        {
            SaveValue("defense", value);
            OnUpdateDefense?.Invoke();
        }
    }

    public int Intelligence
    {
        get => upgrades["intelligence"];
        set
        {
            SaveValue("intelligence", value);
            OnUpdateIntelligence?.Invoke();
        }
    }

    public int Velocity
    {
        get => upgrades["velocity"];
        set
        {
            SaveValue("velocity", value);
            OnUpdateVelocity?.Invoke();
        }
    }

    public int Repairs
    {
        get => upgrades["repairs"];
        set
        {
            SaveValue("repairs", value);
            OnUpdateRepairs?.Invoke();
        }
    }

    public int Stickers
    {
        get => upgrades["stickers"];
        set
        {
            SaveValue("stickers", value);
            OnUpdateStickers?.Invoke();
        }
    }

    public int Smokes
    {
        get => upgrades["smokes"];
        set
        {
            SaveValue("smokes", value);
            OnUpdateSmokes?.Invoke();
        }
    }

    public static RogueItems Instance;

    private void Awake()
    {
        Instance = this;

        upgrades.Add("attack", 0);
        upgrades.Add("defense", 0);
        upgrades.Add("intelligence", 0);
        upgrades.Add("velocity", 0);
        upgrades.Add("repairs", 0);
        upgrades.Add("stickers", 0);
        upgrades.Add("smokes", 0);

        for (int i = 0; i < upgrades.Keys.Count; i++)
        {
            if (PlayerPrefs.HasKey(upgrades.ElementAt(i).Key))
            {
                upgrades[upgrades.ElementAt(i).Key] = PlayerPrefs.GetInt(upgrades.ElementAt(i).Key);
            }
            else PlayerPrefs.SetInt(upgrades.ElementAt(i).Key, upgrades.ElementAt(i).Value);
        }
    }

    private void SaveValue(string key, int value)
    {
        upgrades[key] = value;
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }
}
