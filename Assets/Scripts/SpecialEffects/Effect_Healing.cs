using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "ScriptableObjects/Effects/Heal")]
public class Effect_Healing : Effect
{
    protected override bool ApplyEffect(GameObject emitter, int value, float applicationChance)
    {
        if (!base.ApplyEffect(emitter, value, applicationChance)) return false;

        return true;
    }
}