using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplyFreezingStatusEffect", menuName = "ScriptableObjects/Effects/ApplyFreezingStatusEffect")]
public class Effect_ApplyFreezingStatusEffect : Effect
{
    protected override bool ApplyEffectByChance(Robot emitter, Robot target, int value, float applicationChance, List<EffectSkill> skills, CardData usedCard)
    {
        if (!base.ApplyEffectByChance(emitter, target, value, applicationChance, skills, usedCard))
        {
            return false;
        }

        var newThornStatus = new StatusEffect_Freezing();

        newThornStatus.statusTrigger = StatusEffectTrigger.OnEndRound;
        newThornStatus.Amount = value;

        target.ApplyStatusEffect(newThornStatus);

        return true;
    }
}