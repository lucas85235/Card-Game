using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card", order = 2)]
public class CardData : ScriptableObject
{
    [Header("Visual")]
    [SerializeField] private Sprite cardSprite;
    [SerializeField] private string title;
    [SerializeField] private string description;

    [Header("Stats")]
    [SerializeField] private int energy;
    [SerializeField] private int attack;
    [SerializeField] private int defense;

    public Sprite Sprite() { return cardSprite; }
    public string Title() { return title; }
    public string Description() { return description; }
    public int Energy() { return energy; }
    public int Attack() { return attack; }
    public int Defence() { return defense; }
}
