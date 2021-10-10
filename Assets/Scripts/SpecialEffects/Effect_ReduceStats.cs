using UnityEngine;

[CreateAssetMenu(fileName = "ReduceStats", menuName = "ScriptableObjects/Effects/ReduceStats")]
public class Effect_ReduceStats : Effect
{
    [SerializeField] private Stats statToModify;

    protected override bool ApplyEffect(Robot emitter, Robot target, int value, float applicationChance)
    {
        if(!base.ApplyEffect(emitter, target, value, applicationChance)) return false;

        if (statToModify == Stats.defence)
        {
            target.DefenseDebuff(value);
        }
        if (statToModify == Stats.attack)
        {
            target.AttackDebuff(value);
        }
        if (statToModify == Stats.velocity)
        {
            target.SpeedDebuff(value);
        }

        return true;
    }
}
