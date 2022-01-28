using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplyFreezingStatusEffect", menuName = "ScriptableObjects/Effects/ApplyFreezingStatusEffect")]
public class Effect_ApplyFreezingStatusEffect : Effect
{
    [SerializeField] private Sprite icon;
    [SerializeField] private StatusEffectTrigger effectTrigger;
    protected override bool ApplyEffectByChance(Robot emitter, Robot target, int value, float applicationChance, List<EffectSkill> skills, CardData usedCard)
    {
        if (!base.ApplyEffectByChance(emitter, target, value, applicationChance, skills, usedCard))
        {
            return false;
        }

        var newFreezingStatus = new StatusEffect_Freezing();

        newFreezingStatus.statusTrigger = effectTrigger;
        newFreezingStatus.Amount = value;

        target.ApplyStatusEffect(newFreezingStatus);

        return true;
    }
}