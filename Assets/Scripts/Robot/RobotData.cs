using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Robot", menuName = "ScriptableObjects/RobotData", order = 1)]
public class RobotData : ScriptableObject
{
    [Header("Info")]
    [SerializeField] private string characterName;

    // PRIORIDADE 1

    [Header("Parts")]
    [SerializeField] private Head head;
    [SerializeField] private Torso torso;
    [SerializeField] private RightArm rightArm;
    [SerializeField] private LeftArm leftArm;
    [SerializeField] private Leg leg;

    [Header("Debug")]
    [SerializeField] private Sprite sketchSprite;

    public int Health()
    {
        return head.Health() + torso.Health() + rightArm.Health() + leftArm.Health() + leg.Health();
    }
    public int Energy() 
    { 
        return head.Energy() + torso.Energy() + rightArm.Energy() + leftArm.Energy() + leg.Energy(); 
    }
    public int Attack() 
    {
        return head.Attack() + torso.Attack() + rightArm.Attack() + leftArm.Attack() + leg.Attack(); 
    }
    public int Defense()
    { 
        return head.Defense() + torso.Defense() + rightArm.Defense() + leftArm.Defense() + leg.Defense(); 
    }
    public int Speed()
    {
        return head.Speed() + torso.Speed() + rightArm.Speed() + leftArm.Speed() + leg.Speed(); 
    }

    public List<CardData> Cards()
    {
        var cardList = new List<CardData>();

        foreach (var card in head.Cards()) cardList.Add(card);
        foreach (var card in torso.Cards()) cardList.Add(card);
        foreach (var card in rightArm.Cards()) cardList.Add(card);
        foreach (var card in leftArm.Cards()) cardList.Add(card);
        foreach (var card in leg.Cards()) cardList.Add(card);

        return cardList;
    }

    public Sprite Sprite()
    {
        return sketchSprite;
    }

    // PRIORIDADE 2

    // public int evasion;
    // public int accuracy;
    // public int criticalChance;

    // PRIORIDADE 3

    // public int intelligence;
    // public int fireResist;
    // public int waterResist;
    // public int eletricResist;
    // public int acidResist;

}
