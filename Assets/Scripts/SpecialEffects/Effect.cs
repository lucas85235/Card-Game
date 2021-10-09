using UnityEngine;

public class Effect : ScriptableObject
{
    protected virtual bool ApplyEffect(GameObject emitter, int value, float applicationChance)
    {
        float chance = Random.Range(0f, 1f);
        return chance < applicationChance;
    }
}
