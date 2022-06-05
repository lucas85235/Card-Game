using System.Collections.Generic;

public class StatusEffect_SelfHealing : StatusEffect
{
    public override List<StatusEffect> UpdateStatusList(List<StatusEffect> statusList)
    {
        foreach (var status in statusList)
        {
            if (status.GetType() == typeof(StatusEffect_SelfHealing))
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
            if (affectedRobot.GetType() == typeof(RobotSingleplayer))
                ((RobotSingleplayer)affectedRobot).life.AddLife(Amount);
        }
        Amount--;

        return base.ActivateStatusEffect(affectedRobot, criteria);
    }
}