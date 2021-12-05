using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplyThornStatusEffect", menuName = "ScriptableObjects/Effects/ApplyThornStatusEffect")]
public class Effect_ApplyThornStatusEffect : Effect
{
    protected override bool ApplyEffectByChance(Robot emitter, Robot target, int value, float applicationChance, List<EffectSkill> skills, CardData usedCard)
    {
        if(!base.ApplyEffectByChance(emitter, target, value, applicationChance, skills, usedCard))
        {
            return false;
        }

        var newThornStatus = new StatusEffect_Thorns();

        newThornStatus.statusTrigger = StatusEffectTrigger.OnReceiveDamage;
        newThornStatus.Amount = value;

        target.ApplyStatusEffect(newThornStatus);

        return true;
    }
}