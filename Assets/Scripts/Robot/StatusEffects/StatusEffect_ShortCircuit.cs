using System.Collections.Generic;

public class StatusEffect_ShortCircuit : StatusEffect
{
    public override List<StatusEffect> UpdateStatusList(List<StatusEffect> statusList)
    {
        foreach (var status in statusList)
        {
            if (status.GetType() == typeof(StatusEffect_ShortCircuit))
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
        Amount--;

        return base.ActivateStatusEffect(affectedRobot, criteria);
    }
}