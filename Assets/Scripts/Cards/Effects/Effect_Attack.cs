using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Effects/Attack")]
public class Effect_Attack : Effect
{
    [SerializeField] private Element attackElement;
    [SerializeField] private AttackType attackType;

    protected override bool ApplyEffectByChance(Robot emitter, Robot target, int value, float applicationChance, List<EffectSkill> skills, CardData usedCard)
    {
        if (!base.ApplyEffectByChance(emitter, target, value, applicationChance, skills, usedCard))
        {
            return false;
        }

        if (target != null)
        {
            target.life.TakeDamage(value, attackType, attackElement, usedCard, skills);
        }
        else
        {
            Debug.LogWarning("Target is null");
        }

        return true;
    }
}
