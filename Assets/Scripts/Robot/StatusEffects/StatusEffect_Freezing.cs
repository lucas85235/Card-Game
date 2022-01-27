using System.Collections.Generic;

public class StatusEffect_Freezing : StatusEffect
{
    public override List<StatusEffect> UpdateStatusList(List<StatusEffect> statusList)
    {
        foreach (var status in statusList)
        {
            if (status.GetType() == typeof(StatusEffect_Freezing))
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
        if (Amount > 0)
        {
            affectedRobot.energy.ChangeMaxEnergyAmount(-Amount);
        }
        Amount--;
        return base.ActivateStatusEffect(affectedRobot, criteria);
    }

    public override void StatusEffectPosAction(Robot affectedRobot)
    {
        base.StatusEffectPosAction(affectedRobot);
        affectedRobot.energy.ChangeMaxEnergyAmount(0);
    }
}