using UnityEngine;

[CreateAssetMenu(fileName = "StatsBonus", menuName = "ScriptableObjects/Effects/StatsBonus")]
public class Effect_StatsBonus : Effect
{
    [SerializeField] private Stats statToModify;

    protected override bool ApplyEffect(Robot emitter, Robot target, int value, float applicationChance)
    {
        if (!base.ApplyEffect(emitter, target, value, applicationChance)) return false;

        if (statToModify == Stats.defence)
        {
            emitter.DefenseBuff(value);
        }
        if (statToModify == Stats.attack)
        {
            emitter.AttackBuff(value);
        }

        return true;
    }
}
