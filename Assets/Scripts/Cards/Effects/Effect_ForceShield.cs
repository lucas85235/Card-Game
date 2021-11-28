using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ForceShield", menuName = "ScriptableObjects/Effects/ForceShield")]
public class Effect_ForceShield : Effect
{
    protected override bool ApplyEffectByChance(Robot emitter, Robot target, int value, float applicationChance, List<EffectSkill> skills, CardData usedCard)
    {
        if (emitter == null || !base.ApplyEffectByChance(emitter, target, value, applicationChance, skills, usedCard)) return false;

        emitter.life.AddShield(value);

        return true;
    }
}
