using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Robot", menuName = "ScriptableObjects/RobotData", order = 1)]
public class RobotData : ScriptableObject
{
    [Header("Info")]
    public string characterName;
    public string botFunction;
    public string storyDescription;

    // PRIORIDADE 1

    [Header("Parts")]
    [SerializeField] private Head head;
    [SerializeField] private Torso torso;
    [SerializeField] private RightArm rightArm;
    [SerializeField] private LeftArm leftArm;
    [SerializeField] private Leg leg;

    [Header("Temp Stats")]
    [SerializeField] private int health;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int speed;

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
    public int Inteligence() 
    {
        return head.Inteligence() + torso.Inteligence() + rightArm.Inteligence() + leftArm.Inteligence() + leg.Inteligence();
    }
    public int Accuracy() 
    { 
        return head.Accuracy() + torso.Accuracy() + rightArm.Accuracy() + leftArm.Accuracy() + leg.Accuracy();
    }
    public int Evasion() 
    {
        return head.Evasion() + torso.Evasion() + rightArm.Evasion() + leftArm.Evasion() + leg.Evasion();
    }
    public int CritChance() 
    { 
        return head.CritChance() + torso.CritChance() + rightArm.CritChance() + leftArm.CritChance() + leg.CritChance(); 
    }
    public int FireResistence() 
    {
        return head.FireResistence() + torso.FireResistence() + rightArm.FireResistence() + leftArm.FireResistence() + leg.FireResistence();
    }
    public int IceResistence() 
    {
        return head.WaterResistence() + torso.WaterResistence() + rightArm.WaterResistence() + leftArm.WaterResistence() + leg.WaterResistence();
    }
    public int ElectricResistence() 
    {
        return head.ElectricResistence() + torso.ElectricResistence() + rightArm.ElectricResistence() + leftArm.ElectricResistence() + leg.ElectricResistence();
    }
    public int AcidResistence() 
    {
        return head.AcidResistence() + torso.AcidResistence() + rightArm.AcidResistence() + leftArm.AcidResistence() + leg.AcidResistence();
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

    public List<Sprite> Sprites()
    {
        var spritesList = new List<Sprite>();

        spritesList.Add(torso.Sprite());
        spritesList.Add(leg.Sprite());
        spritesList.Add(head.Sprite());
        spritesList.Add(rightArm.SpecialSprite());
        spritesList.Add(rightArm.Sprite());
        spritesList.Add(leftArm.SpecialSprite());
        spritesList.Add(leftArm.Sprite());

        return spritesList;
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
