using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectSkill_HealByDamage", menuName = "ScriptableObjects/Effects/EffectSkills/HealByDamage")]
public class EffectSkill_HealByDamage : EffectSkill
{
    [SerializeField] private float healByDamagePercentage;

    public override void ApplySkill(Robot emitter, Robot target, int value, CardData usedCard, List<EffectSkill> skills)
    {
        base.ApplySkill(emitter, target, value, usedCard, skills);

        emitter.life.AddLife(Mathf.FloorToInt(value * healByDamagePercentage));
    }
}
