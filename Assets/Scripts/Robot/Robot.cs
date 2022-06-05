using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Robot : MonoBehaviour
{
    [Header("Set Character Data")]
    [SerializeField] protected RobotData m_Data;

    [Header("Protected Debug")]
    [SerializeField] protected bool m_iconSpawInLeft;

    public RobotData Data { get => m_Data; }
    public Dictionary<Stats, int> CurrentRobotStats { get; protected set; } = new Dictionary<Stats, int>();
    public Dictionary<Stats, int> DataStats { get; protected set; } = new Dictionary<Stats, int>();
    public List<CardData> CurrentCards { get; protected set; }
    public List<StatusEffect> StatusList { get; set; } = new List<StatusEffect>();

    public Life life { get; protected set; }
    public Energy energy { get; protected set; }
    protected RobotAnimation m_RobotAnimation;

    public void ApplyStatChange(Stats statToChange, int value)
    {
        CurrentRobotStats[statToChange] += value;
        var textColor = value > 0 ? Color.blue : Color.red;

        GameController.i.ShowAlertText(value, m_iconSpawInLeft, statToChange, textColor);
    }

    public void ApplyStatusEffect(StatusEffect newStatusEffect)
    {
        StatusList = newStatusEffect.UpdateStatusList(StatusList);
    }

    public int StatDiff(Stats statToCompare)
    {
        return CurrentRobotStats[statToCompare] - DataStats[statToCompare];
    }

    public void StatReset(Stats statToReset)
    {
        CurrentRobotStats[statToReset] = DataStats[statToReset];
    }

    public void RemoveCard(CardData cardToRemove)
    {
        CurrentCards.Remove(cardToRemove);
    }

    protected void SetStats()
    {
        CurrentRobotStats[Stats.attack] = m_Data.Attack();
        DataStats[Stats.attack] = m_Data.Attack();

        CurrentRobotStats[Stats.defence] = m_Data.Defense();
        DataStats[Stats.defence] = m_Data.Defense();

        CurrentRobotStats[Stats.speed] = m_Data.Speed();
        DataStats[Stats.speed] = m_Data.Speed();

        CurrentRobotStats[Stats.evasion] = m_Data.Evasion();
        DataStats[Stats.evasion] = m_Data.Evasion();

        CurrentRobotStats[Stats.accuracy] = m_Data.Accuracy();
        DataStats[Stats.accuracy] = m_Data.Accuracy();

        CurrentRobotStats[Stats.inteligence] = m_Data.Inteligence();
        DataStats[Stats.inteligence] = m_Data.Inteligence();

        CurrentRobotStats[Stats.health] = m_Data.Health();
        DataStats[Stats.health] = m_Data.Health();

        CurrentRobotStats[Stats.critChance] = m_Data.CritChance();
        DataStats[Stats.critChance] = m_Data.CritChance();

        CurrentRobotStats[Stats.fireResistence] = m_Data.FireResistence();
        DataStats[Stats.fireResistence] = m_Data.FireResistence();

        CurrentRobotStats[Stats.waterResistence] = m_Data.WaterResistence();
        DataStats[Stats.waterResistence] = m_Data.WaterResistence();

        CurrentRobotStats[Stats.acidResistence] = m_Data.AcidResistence();
        DataStats[Stats.acidResistence] = m_Data.AcidResistence();

        CurrentRobotStats[Stats.electricResistence] = m_Data.ElectricResistence();
        DataStats[Stats.electricResistence] = m_Data.ElectricResistence();
    }

    public bool ActivateEarlyStatusEffects()
    {
        var toRemoveInStatusList = new List<StatusEffect>();

        foreach (var status in StatusList)
        {
            if (status.statusTrigger == StatusEffectTrigger.OnStartRound && status.ActivateStatusEffect(this))
            {
                toRemoveInStatusList.Add(status);
            }
        }

        StatusList = StatusList.Except(toRemoveInStatusList).ToList();

        return true;
    }

    public async Task<bool> ActivateLateStatusEffects(int timeBetweenStatusEffects)
    {
        var toRemoveInStatusList = new List<StatusEffect>();

        foreach (var status in StatusList)
        {
            if (status.statusTrigger == StatusEffectTrigger.OnEndRound && status.ActivateStatusEffect(this))
            {
                await Task.Delay(timeBetweenStatusEffects);
                toRemoveInStatusList.Add(status);
            }
        }

        StatusList = StatusList.Except(toRemoveInStatusList).ToList();

        return true;
    }
}
