using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObjects/Effects/Heal")]
public class Effect_Healing : Effect
{
    protected override bool ApplyEffectByChance(Robot emitter, Robot target, int value, float applicationChance, List<EffectSkill> skills)
    {
        if (!base.ApplyEffectByChance(emitter, target, value, applicationChance, skills)) return false;

        emitter.life.AddLife(value);

        return true;
    }
}