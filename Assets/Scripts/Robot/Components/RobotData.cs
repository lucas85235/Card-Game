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

    #region PartsGetsAndSets

    public Head GetHead() => head;
    public Torso GetTorso() => torso;
    public RightArm GetRightArm() => rightArm;
    public LeftArm GetLeftArm() => leftArm;
    public Leg GetLeg() => leg;
    
    public void SetHead(Head _head) => head = _head;
    public void SetTorso(Torso _torso) => torso = _torso;
    public void SetRightArm(RightArm _rightArm) => rightArm = _rightArm;
    public void SetLeftArm(LeftArm _leftArm) => leftArm = _leftArm;
    public void SetLeg(Leg _leg) => leg = _leg;

    #endregion

    #region PartsStats

    public int Health() => head.Health() + torso.Health() + rightArm.Health() + leftArm.Health() + leg.Health();
    public int Energy() => head.Energy() + torso.Energy() + rightArm.Energy() + leftArm.Energy() + leg.Energy(); 
    public int Attack()  => head.Attack() + torso.Attack() + rightArm.Attack() + leftArm.Attack() + leg.Attack(); 
    public int Defense() => head.Defense() + torso.Defense() + rightArm.Defense() + leftArm.Defense() + leg.Defense(); 
    public int Speed() => head.Speed() + torso.Speed() + rightArm.Speed() + leftArm.Speed() + leg.Speed();
    public int Inteligence()  => head.Inteligence() + torso.Inteligence() + rightArm.Inteligence() + leftArm.Inteligence() + leg.Inteligence();
    public int Accuracy() => head.Accuracy() + torso.Accuracy() + rightArm.Accuracy() + leftArm.Accuracy() + leg.Accuracy();
    public int Evasion()  => head.Evasion() + torso.Evasion() + rightArm.Evasion() + leftArm.Evasion() + leg.Evasion();
    public int CritChance() => head.CritChance() + torso.CritChance() + rightArm.CritChance() + leftArm.CritChance() + leg.CritChance(); 
    public int FireResistence()  => head.FireResistence() + torso.FireResistence() + rightArm.FireResistence() + leftArm.FireResistence() + leg.FireResistence();
    public int WaterResistence()  => head.WaterResistence() + torso.WaterResistence() + rightArm.WaterResistence() + leftArm.WaterResistence() + leg.WaterResistence();
    public int ElectricResistence()  => head.ElectricResistence() + torso.ElectricResistence() + rightArm.ElectricResistence() + leftArm.ElectricResistence() + leg.ElectricResistence();
    public int AcidResistence()  => head.AcidResistence() + torso.AcidResistence() + rightArm.AcidResistence() + leftArm.AcidResistence() + leg.AcidResistence();
    
    #endregion

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

    public Dictionary<string, Sprite> Sprites()
    {
        var spritesList = new Dictionary<string, Sprite>();

        spritesList.Add("head", head.Sprite());
        spritesList.Add("head-special", head.SpecialSprite());

        spritesList.Add("torso", torso.Sprite());
        spritesList.Add("torso-special", torso.SpecialSprite());

        spritesList.Add("leg-bowl", leg.Sprite());
        spritesList.Add("leg-right", leg.RightLeg());
        spritesList.Add("hoot-right", leg.RightFoot());
        spritesList.Add("leg-left", leg.LeftLeg());
        spritesList.Add("foot-left", leg.LeftFoot());

        spritesList.Add("right-arm-shouder", rightArm.Sprite());
        spritesList.Add("right-arm", rightArm.ArmSprite());
        if ( rightArm.HandSprite() != null )
            spritesList.Add("right-hand", rightArm.HandSprite());

        spritesList.Add("left-arm-shouder", leftArm.Sprite());
        spritesList.Add("left-arm", leftArm.ArmSprite());
        if ( leftArm.HandSprite() != null )
            spritesList.Add("left-hand", leftArm.HandSprite());

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
