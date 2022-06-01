using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueEvent : MonoBehaviour
{
    public EventPanelUI eventUI;

    public static RogueEvent Instance;

    private void Awake() 
    {
        Instance = this;
    }

    private void Start()
    {
        RogueManager.Instance.Start();
    }
}
