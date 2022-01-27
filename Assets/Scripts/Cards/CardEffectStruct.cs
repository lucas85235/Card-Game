using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CardEffectStruct
{
    [Header("Skills")]
    [SerializeField] private List<EffectSkill> skills;

    [Header("Effect")]
    public Effect effect;
    public Vector2Int value;
    public float chance;
    public bool selfApply;

    [Header("Bonus")]
    public bool effectBonusByStat;
    public Stats statToApplyEffectBonus;
    public Vector2Int effectMultiplierRange;

    public void UseEffect(Robot emitter, Robot target, CardData usedData)
    {
        var isRand = value.x != value.y;
        var rand = Random.Range(value.x, value.y);

        effectMultiplierRange.x = effectMultiplierRange.x < 1 ? 1 : effectMultiplierRange.x;
        var finalValue = (isRand ? rand : value.x) * effectMultiplierRange.x;

        Robot finalTarget = selfApply ? emitter : target;

        effect.UseEffect(emitter, finalTarget, finalValue, chance, skills, usedData);
    }
}
