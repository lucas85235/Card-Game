using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect
{
    public Sprite icon;
    public StatusEffectTrigger statusTrigger;

    public int Amount { get; set; }

    public virtual List<StatusEffect> UpdateStatusList(List<StatusEffect> statusList)
    {
        return statusList;
    }

    public virtual bool ActivateStatusEffect(Robot affectedRobot, AttackType criteria = AttackType.none)
    {
        if(Amount <= 0)
        {
            StatusEffectPosAction(affectedRobot);
            return true;
        }
        return false;
    }

    public virtual void StatusEffectPosAction(Robot affectedRobot)
    {

    }
}
