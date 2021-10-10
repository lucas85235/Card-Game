using UnityEngine;

[System.Serializable]
public class Effect : ScriptableObject
{
    public bool UseEffect(Robot emitter, Robot target, int value, float applicationChance)
    {
        return ApplyEffect(emitter, target, value, applicationChance);
    }

    protected virtual bool ApplyEffect(Robot emitter, Robot target, int value, float applicationChance)
    {
        float chance = Random.Range(0f, 1f);
        return chance < applicationChance;
    }
}
