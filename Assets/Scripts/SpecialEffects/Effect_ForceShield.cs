using UnityEngine;

[CreateAssetMenu(fileName = "ForceShield", menuName = "ScriptableObjects/Effects/ForceShield")]
public class Effect_ForceShield : Effect
{
    protected override bool ApplyEffect(Robot emitter, Robot target, int value, float applicationChance)
    {
        if (emitter == null || !base.ApplyEffect(emitter, target, value, applicationChance)) return false;

        emitter.life.AddShild(value);

        return true;
    }
}
