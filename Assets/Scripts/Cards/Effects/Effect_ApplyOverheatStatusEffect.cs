using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplyOverheatStatusEffect", menuName = "ScriptableObjects/Effects/ApplyOverheatStatusEffect")]
public class Effect_ApplyOverheatStatusEffect : Effect
{
    protected override bool ApplyEffectByChance(Robot emitter, Robot target, int value, float applicationChance, List<EffectSkill> skills, CardData usedCard)
    {
        if (!base.ApplyEffectByChance(emitter, target, value, applicationChance, skills, usedCard))
        {
            return false;
        }

        var newThornStatus = new StatusEffect_Overheat();

        newThornStatus.statusTrigger = StatusEffectTrigger.OnEndRound;
        newThornStatus.Amount = value;

        target.ApplyStatusEffect(newThornStatus);

        return true;
    }
}