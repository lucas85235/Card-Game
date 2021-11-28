using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReduceStats", menuName = "ScriptableObjects/Effects/ReduceStats")]
public class Effect_ReduceStats : Effect
{
    [SerializeField] private Stats statToModify;

    protected override bool ApplyEffectByChance(Robot emitter, Robot target, int value, float applicationChance, List<EffectSkill> skills)
    {
        if(!base.ApplyEffectByChance(emitter, target, value, applicationChance, skills)) return false;

        if (statToModify == Stats.defence)
        {
            target.ApplyDefenceChange(-value);
        }
        if (statToModify == Stats.attack)
        {
            target.ApplyAttackChange(-value);
        }
        if (statToModify == Stats.velocity)
        {
            target.ApplySpeedChange(-value);
        }

        return true;
    }
}
