using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class RogueConsumables : MonoBehaviour
{
    [Space]
    public UnityEvent OnUpdateRepairs;
    public UnityEvent OnUpdateStickers;
    public UnityEvent OnUpdateSmokes;

    private int repairs;
    private int stickers;
    private int smokes;

    public int Repairs
    {
        get => repairs;
        set
        {
            repairs = value;
            PlayerPrefs.GetInt("repairs", repairs);
            PlayerPrefs.Save();
            OnUpdateRepairs?.Invoke();
        }
    }

    public int Stickers
    {
        get => stickers;
        set
        {
            stickers = value;
            PlayerPrefs.GetInt("stickers", stickers);
            PlayerPrefs.Save();
            OnUpdateRepairs?.Invoke();
        }
    }

    public int Smokes
    {
        get => smokes;
        set
        {
            smokes = value;
            PlayerPrefs.GetInt("smokes", smokes);
            PlayerPrefs.Save();
            OnUpdateRepairs?.Invoke();
        }
    }

    public static RogueConsumables Instance;

    private void Awake()
    {
        Instance = this;

        if (!PlayerPrefs.HasKey("repairs"))
            Repairs = PlayerPrefs.GetInt("repairs");

        if (!PlayerPrefs.HasKey("stickers"))
            Stickers = PlayerPrefs.GetInt("stickers");

        if (!PlayerPrefs.HasKey("smokes"))
            Smokes = PlayerPrefs.GetInt("smokes");
    }
}
