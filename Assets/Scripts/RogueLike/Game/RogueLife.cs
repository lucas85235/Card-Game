using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RogueLife : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private int maxLife;
    [SerializeField] private int currentLife;

    [Space]
    [Header("Events")]
    public UnityEvent OnUpdateLife;
    public UnityEvent OnUpdateMaxLife;

    [HideInInspector]
    public bool isReady = false;
    private string saveKey = "SAVE-LIFE";

    public int Life
    {
        get => currentLife;
        set
        {
            currentLife = value;
            OnUpdateLife?.Invoke();
        }
    }

    public int MaxLife
    {
        get => maxLife;
        set
        {
            maxLife = value;
            OnUpdateMaxLife?.Invoke();
        }
    }

    public string RobotName => DataManager.Instance.GetCurrentRobot().characterName;

    public static RogueLife Instance;

    private void Awake()
    {
        Instance = this;

        OnUpdateLife.AddListener(() => 
        {
            PlayerPrefs.SetInt(saveKey, Life);
            PlayerPrefs.Save();
        });
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey(saveKey))
        {
            int healt = DataManager.Instance.GetCurrentRobot().Health();
            MaxLife = healt;
            Life = healt;
        }
        else
        {
            int healt = PlayerPrefs.GetInt(saveKey);
            MaxLife = DataManager.Instance.GetCurrentRobot().Health();
            Life = healt;
        }

        isReady = true;
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey(saveKey);
    }
}
