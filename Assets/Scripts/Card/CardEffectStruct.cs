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
    
    public void UseEffect(Robot emitter, Robot target = null, bool useShild = false)
    {
        bool shildType = 
            effect.GetType() == typeof(Effect_ForceShield);

        if (useShild == false && shildType == true) return;

        if (effect.GetType() == typeof(Effect_StatsBonus))
        {
            effect.UseEffect(emitter, target, value.x, chance);
            return;
        }

        var isRand = value.x != value.y;
        var rand = Random.Range(value.x, value.y);

        effectMultiplierRange.x = effectMultiplierRange.x == 0 ? 1 : effectMultiplierRange.x;
        var finalValue = (isRand ? rand : value.x) * effectMultiplierRange.x;

        effect.UseEffect(emitter, target, finalValue, chance);
    }
}
