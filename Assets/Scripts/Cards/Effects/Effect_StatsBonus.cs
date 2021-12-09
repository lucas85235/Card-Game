using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsBonus", menuName = "ScriptableObjects/Effects/StatsBonus")]
public class Effect_StatsBonus : Effect
{
    [SerializeField] private Stats statToModify;

    protected override bool ApplyEffectByChance(Robot emitter, Robot target, int value, float applicationChance, List<EffectSkill> skills, CardData usedCard)
    {
        if (!base.ApplyEffectByChance(emitter, target, value, applicationChance, skills, usedCard)) return false;

        if (statToModify == Stats.defence)
        {
            emitter.ApplyDefenceChange(value);
        }
        if (statToModify == Stats.attack)
        {
            emitter.ApplyAttackChange(value);
        }
        if (statToModify == Stats.velocity)
        {
            emitter.ApplySpeedChange(value);
        }
        if (statToModify == Stats.critChance)
        {
            emitter.ApplyCritChanceChange(value);
        }
        if (statToModify == Stats.accuracy)
        {
            emitter.ApplyAccuracyChange(value);
        }
        if (statToModify == Stats.evasion)
        {
            emitter.ApplyEvasionChange(value);
        }

        return true;
    }
}
