using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect : ScriptableObject
{
    public bool UseEffect(Robot emitter, Robot target, int value, float applicationChance, List<EffectSkill> skills, CardData usedCard)
    {
        return ApplyEffectByChance(emitter, target, value, applicationChance, skills, usedCard);
    }

    protected virtual bool ApplyEffectByChance(Robot emitter, Robot target, int value, float applicationChance, List<EffectSkill> skills, CardData usedCard)
    {
        float chance = Random.Range(0f, 1f);
        return chance < applicationChance;
    }
}
