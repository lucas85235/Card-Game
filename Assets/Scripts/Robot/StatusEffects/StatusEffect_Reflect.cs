using System.Collections.Generic;

public class StatusEffect_Reflect : StatusEffect
{
    public override List<StatusEffect> UpdateStatusList(List<StatusEffect> statusList)
    {
        foreach (var status in statusList)
        {
            if (status.GetType() == typeof(StatusEffect_Reflect))
            {
                status.Amount += this.Amount;
                return base.UpdateStatusList(statusList);
            }
        }

        statusList.Add(this);
        return base.UpdateStatusList(statusList);
    }

    public override bool ActivateStatusEffect(Robot affectedRobot, AttackType criteria)
    {
        if (criteria == AttackType.ranged)
        {
            GameController.i.GetTheOtherRobot(affectedRobot).life.TakeDamage(Amount, AttackType.none);
        }

        return base.ActivateStatusEffect(affectedRobot, criteria);
    }
}