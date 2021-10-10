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
        if (useShild)
        {
            if (effect.GetType() == typeof(Effect_ForceShield))
            {
                effect.UseEffect(emitter, target, value.x, chance);
            }
            else if (effect.GetType() == typeof(Effect_StatsBonus))
            {
                effect.UseEffect(emitter, target, value.x, chance);
            }
            else if (effect.GetType() == typeof(Effect_ReduceStats))
            {
                effect.UseEffect(emitter, target, value.x, chance);
            }

            return;
        }

        if (effect.GetType() == typeof(Effect_ForceShield))
            return;

        if (effect.GetType() == typeof(Effect_StatsBonus))
            return;

        if (effect.GetType() == typeof(Effect_ReduceStats))
            return;

        var isRand = value.x != value.y;
        var rand = Random.Range(value.x, value.y);

        effectMultiplierRange.x = effectMultiplierRange.x < 1 ? 1 : effectMultiplierRange.x;
        var finalValue = (isRand ? rand : value.x) * effectMultiplierRange.x;

        effect.UseEffect(emitter, target, finalValue, chance);

    }
}
