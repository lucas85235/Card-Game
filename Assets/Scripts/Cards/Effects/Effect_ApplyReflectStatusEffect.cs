using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplyReflectStatusEffect", menuName = "ScriptableObjects/Effects/ApplyReflectStatusEffect")]
public class Effect_ApplyReflectStatusEffect : Effect
{
    [SerializeField] private Sprite icon;
    [SerializeField] private StatusEffectTrigger effectTrigger;
    protected override bool ApplyEffectByChance(Robot emitter, Robot target, int value, float applicationChance, List<EffectSkill> skills, CardData usedCard)
    {
        if (!base.ApplyEffectByChance(emitter, target, value, applicationChance, skills, usedCard))
        {
            return false;
        }

        var newReflectStatus = new StatusEffect_Reflect();

        newReflectStatus.statusTrigger = effectTrigger;
        newReflectStatus.Amount = value;

        target.ApplyStatusEffect(newReflectStatus);

        return true;
    }
}

