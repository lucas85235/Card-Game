using System.Collections.Generic;
using UnityEngine;

public class RobotPart : ScriptableObject
{
    [Header("Visual")]
    [SerializeField] private Sprite partSprite;

    [Header("Stats")]
    [SerializeField] private int health;
    [SerializeField] private int energy;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int speed;

    [Header("Cards")]
    [SerializeField] private List<CardData> cards;

    public int Health(){return health;}
    public int Energy(){return energy;}
    public int Attack(){return attack;}
    public int Defense(){return defense;}
    public int Speed(){return speed;}

    public List<CardData> Cards() { return cards; }
}
