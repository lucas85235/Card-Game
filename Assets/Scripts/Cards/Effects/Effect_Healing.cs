using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObjects/Effects/Heal")]
public class Effect_Healing : Effect
{
    protected override bool ApplyEffectByChance(Robot emitter, Robot target, int value, float applicationChance, List<EffectSkill> skills, CardData usedCard)
    {
        if (!base.ApplyEffectByChance(emitter, target, value, applicationChance, skills, usedCard)) return false;

        if (emitter.GetType() == typeof(RobotSingleplayer))
            ((RobotSingleplayer)emitter).life.AddLife(value);
        // else
        // {
        //     ((Multiplayer.RobotMultiplayer)target).life.AddLife(value);
        //     ((Multiplayer.RobotMultiplayer)target).HealingFeedback(value, target.transform.localScale.x > 0);
        // }

        return true;
    }
}