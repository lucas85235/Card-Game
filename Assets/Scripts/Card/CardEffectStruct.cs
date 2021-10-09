using UnityEngine;

[System.Serializable]
public struct CardEffectStruct
{
    [Header("Effect")]
    public Effect effect;
    public Vector2Int value;
    public float chance;
    public bool selfApply;

    [Header("Bonus")]
    public bool effectBonusByStat;
    public Stats statToApplyEffectBonus;
    public Vector2Int effectMultiplierRange;
    
    public void UseEffect(Robot emitter, Robot target = null)
    {
        effect.UseEffect(emitter, target, value.x, chance);
    }
}
