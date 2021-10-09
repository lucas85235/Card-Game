using UnityEngine;

[CreateAssetMenu(fileName = "ForceShield", menuName = "ScriptableObjects/Effects/ForceShield")]
public class Effect_ForceShield : Effect
{
    protected override bool ApplyEffect(GameObject emitter, int value, float applicationChance)
    {
        if (!base.ApplyEffect(emitter, value, applicationChance)) return false;

        return true;
    }
}
