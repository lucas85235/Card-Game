using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Card_", menuName = "ScriptableObjects/Card", order = 2)]
public class CardData : ScriptableObject
{
    [Header("Visual")]
    [SerializeField] private Sprite cardSprite;
    [SerializeField] private string textKey;

    [Header("Costs")]
    [SerializeField] private int energy;

    [Header("SpecialEffects")]
    [SerializeField] private List<CardEffectStruct> effects;
    [SerializeField] private List<CardSkill> skills;
    [SerializeField] private CardTriggersList trigger;

    public int Priority { get; set; } = 2;
    public float MissChance { get; set; } = 0;
    public float CriticalChance { get; set; } = 0;
    public bool Piercing { get; set; } = false;
    public bool SingleUse { get; set; } = false;

    public Sprite Sprite() { return cardSprite; }
    public string TitleKey() { return "Card_" + textKey + "_Title"; }
    public string DescriptionKey() { return "Card_" + textKey + "_Description"; }
    public int Energy() { return energy; }
    public List<CardEffectStruct> Effects() { return effects; }
    public List<CardSkill> Skills() { return skills; }
    public CardTriggersList Trigger() { return trigger; }
}
