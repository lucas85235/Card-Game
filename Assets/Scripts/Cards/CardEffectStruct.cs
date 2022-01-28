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
        var randomValue = Random.Range(value.x, value.y);
        var finalValue = randomValue;

        if (effectBonusByStat)
        {
            var randomMultiplier = Random.Range(effectMultiplierRange.x, effectMultiplierRange.y);
            finalValue += emitter.CurrentRobotStats[statToApplyEffectBonus] * randomMultiplier;
        }

        Robot finalTarget = selfApply ? emitter : target;

        effect.UseEffect(emitter, finalTarget, finalValue, chance, skills, usedData);
    }
}
