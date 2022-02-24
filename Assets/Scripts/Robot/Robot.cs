using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;
using System.Linq;

[RequireComponent(typeof(Life))]
[RequireComponent(typeof(Energy))]

public class Robot : MonoBehaviour
{
    [Header("Set Character Data")]
    [SerializeField] private RobotData m_Data;
    public Transform selectedCardsConteriner;
    [SerializeField] private bool getFromDataManager;

    [Header("Rand Robot Data")]
    public bool randData = false;
    public RobotData[] datas;

    public List<StatusEffect> StatusList { get; set; } = new List<StatusEffect>();

    public Life life { get; private set; }
    public Energy energy { get; private set; }

    public Dictionary<Stats, int> CurrentRobotStats { get; private set; } = new Dictionary<Stats, int>();
    public Dictionary<Stats, int> DataStats { get; private set; } = new Dictionary<Stats, int>();
    public List<CardData> CurrentCards { get; private set; }

    private RobotAnimation m_RobotAnimation;
    private bool m_iconSpawInLeft;

    private void Awake()
    {
        if (randData && datas.Length > 1)
        {
            m_Data = datas[Random.Range(0, datas.Length)];
        }
        life = GetComponent<Life>();
        energy = GetComponent<Energy>();

        TryGetComponent(out m_RobotAnimation);

        if (getFromDataManager)
        {
            m_Data = DataManager.Instance.GetCurrentRobot();
        }
        m_RobotAnimation.ChangeRobotSprites(m_Data);

        m_iconSpawInLeft = transform.localScale.x > 0;

        GetComponent<RobotAnimation>().ChangeRobotSprites(m_Data);

        SetStats();
        CurrentCards = m_Data.Cards();
    }

    private void Start()
    {
        RemoveAllBuffAndDebuff();

        Round.i.EndTurn.AddListener(() =>
        {
            ActivateEarlyStatusEffects();
        });

        Round.i.StartTurn.AddListener(() => {

            // remove this logic of here
            // because the buff and defuff
            // can be apply for more of one round

            //RemoveAllBuffAndDebuff();
        });

        // Add Robot Attack Feedback To Event List
        Round.i.RobotAttack.AddListener((robot, target) =>
           RobotAttackFeedback(robot, target)
        );
    }

    private void SetStats()
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

    /// <summary>Set Robot Behaviour After Attack Event Called</summary>
    private void RobotAttackFeedback(Robot robot, Robot target)
    {
        if (robot != this) return;
        m_RobotAnimation.PlayAnimation(Animations.action);
    }

    private void RemoveAllBuffAndDebuff()
    {
        StatReset(Stats.attack);
        StatReset(Stats.defence);
        StatReset(Stats.speed);
    }

    public void ApplyStatChange(Stats statToChange, int value)
    {
        CurrentRobotStats[statToChange] += value;

        Debug.Log("VALUE: " + value);

        // var textColor = value > 0 ? Color.blue : Color.red;
        // GameController.i.ShowAlertText(value, m_iconSpawInLeft, statToChange, textColor);
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

    public void ApplyStatusEffect(StatusEffect newStatusEffect)
    {
        StatusList = newStatusEffect.UpdateStatusList(StatusList);
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
