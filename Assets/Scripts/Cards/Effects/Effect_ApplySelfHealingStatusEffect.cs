using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplySelfHealingStatusEffect", menuName = "ScriptableObjects/Effects/ApplySelfHealingStatusEffect")]
public class Effect_ApplySelfHealingStatusEffect : Effect
{
    [SerializeField] private Sprite icon;
    [SerializeField] private StatusEffectTrigger effectTrigger;

    protected override bool ApplyEffectByChance(Robot emitter, Robot target, int value, float applicationChance, List<EffectSkill> skills, CardData usedCard)
    {
        if (!base.ApplyEffectByChance(emitter, target, value, applicationChance, skills, usedCard))
        {
            return false;
        }

        var newSelfHealingStatus = new StatusEffect_SelfHealing();

        newSelfHealingStatus.statusTrigger = effectTrigger;
        newSelfHealingStatus.Amount = value;

        target.ApplyStatusEffect(newSelfHealingStatus);

        return true;
    }
}
