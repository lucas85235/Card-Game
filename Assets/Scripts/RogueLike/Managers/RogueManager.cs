using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RoguePath _roguePath;
    [SerializeField] private RogueEvent _rogueEvent;
    [SerializeField] private RogueLife _rogueLife;

    public static RogueManager Instance { get; set; }
    public static RoguePath RoguePath { get => Instance._roguePath; }
    public static RogueEvent RogueEvent { get => Instance._rogueEvent; }
    public static RogueLife RogueLife { get => Instance._rogueLife; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start() 
    {
        if (_roguePath == null)
        {
            var findPath = FindObjectOfType<RoguePath>();
            if (findPath != null)
                _roguePath = findPath;
        }
        if (_rogueEvent == null)
        {
            var findEvents = FindObjectOfType<RogueEvent>();
            if (findEvents != null)
                _rogueEvent = findEvents;
        }
        if (_rogueLife == null)
        {
            var findLife = FindObjectOfType<RogueLife>();
            if (findLife != null)
                _rogueLife = findLife;
        }

        if (_roguePath != null)
        {
            _roguePath.OnUpdatePoint.AddListener(OnUpdatePathEvent);
        }

        RogueLife.OnUpdateMaxLife?.Invoke();
        RogueLife.OnUpdateLife?.Invoke();
    }

    private void OnUpdatePathEvent(RoguePathPoints point)
    {
        RogueEvent.eventUI.SetEventPanel(point);
    }

    public void OnApplicationQuit()
    {
        DeleteKeys();
    }

    public void OnDestroy()
    {
        if (Instance == this)
        {
            DeleteKeys();   
        }
    }

    private void DeleteKeys()
    {
        PlayerPrefs.DeleteKey("SAVE_POINT");
        PlayerPrefs.DeleteKey("SAVE-LIFE");

        PlayerPrefs.DeleteKey("attack");
        PlayerPrefs.DeleteKey("defense");
        PlayerPrefs.DeleteKey("intelligence");
        PlayerPrefs.DeleteKey("velocity");
        PlayerPrefs.DeleteKey("repairs");
        PlayerPrefs.DeleteKey("stickers");
        PlayerPrefs.DeleteKey("smokes");
    }
}
