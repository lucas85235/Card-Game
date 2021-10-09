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

    [Header("Costs")]
    [SerializeField] private int energy;

    [Header("SpecialEffects")]
    [SerializeField] private List<CardEffectStruct> effects;

    public Sprite Sprite() { return cardSprite; }
    public string Title() { return title; }
    public string Description() { return description; }
    public int Energy() { return energy; }
    public List<CardEffectStruct> Effects() { return effects; }
}
