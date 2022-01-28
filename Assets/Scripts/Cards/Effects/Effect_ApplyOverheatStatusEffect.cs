using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplyOverheatStatusEffect", menuName = "ScriptableObjects/Effects/ApplyOverheatStatusEffect")]
public class Effect_ApplyOverheatStatusEffect : Effect
{
    [SerializeField] private Sprite icon;
    [SerializeField] private StatusEffectTrigger effectTrigger;

    protected override bool ApplyEffectByChance(Robot emitter, Robot target, int value, float applicationChance, List<EffectSkill> skills, CardData usedCard)
    {
        if (!base.ApplyEffectByChance(emitter, target, value, applicationChance, skills, usedCard))
        {
            return false;
        }

        var newOverheatStatus = new StatusEffect_Overheat();

        newOverheatStatus.statusTrigger = effectTrigger;
        newOverheatStatus.Amount = value;

        target.ApplyStatusEffect(newOverheatStatus);

        return true;
    }
}