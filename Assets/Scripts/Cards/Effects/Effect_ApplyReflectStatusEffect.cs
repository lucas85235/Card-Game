using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplyReflectStatusEffect", menuName = "ScriptableObjects/Effects/ApplyReflectStatusEffect")]
public class Effect_ApplyReflectStatusEffect : Effect
{
    protected override bool ApplyEffectByChance(Robot emitter, Robot target, int value, float applicationChance, List<EffectSkill> skills, CardData usedCard)
    {
        if (!base.ApplyEffectByChance(emitter, target, value, applicationChance, skills, usedCard))
        {
            return false;
        }

        var newThornStatus = new StatusEffect_Reflect();

        newThornStatus.statusTrigger = StatusEffectTrigger.OnReceiveDamage;
        newThornStatus.Amount = value;

        target.ApplyStatusEffect(newThornStatus);

        return true;
    }
}

