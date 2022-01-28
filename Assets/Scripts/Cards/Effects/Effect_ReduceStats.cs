using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReduceStats", menuName = "ScriptableObjects/Effects/ReduceStats")]
public class Effect_ReduceStats : Effect
{
    [SerializeField] private Stats statToModify;

    protected override bool ApplyEffectByChance(Robot emitter, Robot target, int value, float applicationChance, List<EffectSkill> skills, CardData usedCard)
    {
        if(!base.ApplyEffectByChance(emitter, target, value, applicationChance, skills, usedCard)) return false;

        target.ApplyStatChange(statToModify, -value);

        return true;
    }
}
