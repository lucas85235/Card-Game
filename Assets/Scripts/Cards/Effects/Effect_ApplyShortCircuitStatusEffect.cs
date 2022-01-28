using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplyShortCircuitStatusEffect", menuName = "ScriptableObjects/Effects/ApplyShortCircuitStatusEffect")]
public class Effect_ApplyShortCircuitStatusEffect : Effect
{
    [SerializeField] private Sprite icon;
    [SerializeField] private StatusEffectTrigger effectTrigger;
    protected override bool ApplyEffectByChance(Robot emitter, Robot target, int value, float applicationChance, List<EffectSkill> skills, CardData usedCard)
    {
        if (!base.ApplyEffectByChance(emitter, target, value, applicationChance, skills, usedCard))
        {
            return false;
        }

        var newShortCircuitStatus = new StatusEffect_ShortCircuit();

        newShortCircuitStatus.statusTrigger = effectTrigger;
        newShortCircuitStatus.Amount = value;

        target.ApplyStatusEffect(newShortCircuitStatus);

        return true;
    }
}