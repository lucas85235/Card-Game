using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplyCorrodeStatusEffect", menuName = "ScriptableObjects/Effects/ApplyCorrodeStatusEffect")]
public class Effect_ApplyCorrodeStatusEffect : Effect
{
    [SerializeField] private Sprite icon;
    [SerializeField] private StatusEffectTrigger effectTrigger;

    protected override bool ApplyEffectByChance(Robot emitter, Robot target, int value, float applicationChance, List<EffectSkill> skills, CardData usedCard)
    {
        if (!base.ApplyEffectByChance(emitter, target, value, applicationChance, skills, usedCard))
        {
            return false;
        }

        var newCorrodeStatus = new StatusEffect_Corrode();

        newCorrodeStatus.statusTrigger = effectTrigger;
        newCorrodeStatus.Amount = value;

        target.ApplyStatusEffect(newCorrodeStatus);

        return true;
    }
}