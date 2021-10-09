using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Robot", menuName = "ScriptableObjects/Robot", order = 2)]
public class RobotData : ScriptableObject
{
    [Header("Info")]
    public string characterName;

    // PRIORIDADE 1

    [Header("Stats")]
    public int health;
    public int energy;
    public int attack;
    public int defense;
    public int speed;

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
