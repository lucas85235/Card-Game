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
            var damage = emitter.CurrentRobotStats[Stats.attack] + value - target.CurrentRobotStats[Stats.defence];
            damage = damage < 1 ? 0 : damage; 

            // Debug.Log("ATTACK: " + emitter.Attack());
            // Debug.Log("RAND: " + value);
            // Debug.Log("DEF: " + emitter.Defense());
            // Debug.Log("DAMAGE: " + damage);

            target.life.TakeDamage(damage, attackType, attackElement, usedCard, skills);
        }
        else
        {
            Debug.LogWarning("Target is null");
        }

        return true;
    }
}
