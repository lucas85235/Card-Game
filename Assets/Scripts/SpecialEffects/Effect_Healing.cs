using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObjects/Effects/Heal")]
public class Effect_Healing : Effect
{
    protected override bool ApplyEffect(Robot emitter, Robot target, int value, float applicationChance)
    {
        if (!base.ApplyEffect(emitter, target, value, applicationChance)) return false;

        return true;
    }
}