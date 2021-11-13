using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RobotPart : ScriptableObject
{
    [SerializeField] private string id;

    [Header("Visual")]
    [SerializeField] private Sprite partSprite;

    [Header("Primary Stats")]
    [SerializeField] private int health;
    [SerializeField] private int energy;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int speed;

    [Header("Secondary Stats")]
    [SerializeField] private int inteligence;
    [SerializeField] private int accuracy;
    [SerializeField] private int evasion;
    [SerializeField] private int critChance;

    [Header("Resistences")]
    [SerializeField] private int fireResistence;
    [SerializeField] private int waterResistence;
    [SerializeField] private int electricResistence;
    [SerializeField] private int acidResistence;

    [Header("Cards")]
    [SerializeField] private List<CardData> cards;

    public string ID() { return id; }

    public int Health() { return health; }
    public int Energy() { return energy; }
    public int Attack() { return attack; }
    public int Defense() { return defense; }
    public int Speed() { return speed; }
    public int Inteligence() { return inteligence; }
    public int Accuracy() { return accuracy; }
    public int Evasion() { return evasion; }
    public int CritChance() { return critChance; }
    public int FireResistence() { return fireResistence; }
    public int WaterResistence() { return waterResistence; }
    public int ElectricResistence() { return electricResistence; }
    public int AcidResistence() { return acidResistence; }

    public List<CardData> Cards() { return cards; }
    public Sprite Sprite() { return partSprite; }
}
