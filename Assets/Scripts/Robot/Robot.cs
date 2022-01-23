using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

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

    [Header("CURRENT")]
    [SerializeField] private int m_currentAttack;
    [SerializeField] private int m_currentDefense;
    [SerializeField] private int m_currentSpeed;
    [SerializeField] private int m_currentCrit;
    [SerializeField] private int m_currentEvasion;
    [SerializeField] private int m_currentAccuracy;
    [SerializeField] private int m_currentInteligence;
    [SerializeField] private int m_currentFireResistence;
    [SerializeField] private int m_currentWaterResistence;
    [SerializeField] private int m_currentElectricResistence;
    [SerializeField] private int m_currentAcidResistence;

    private bool m_iconSpawInLeft;

    public List<StatusEffect> StatusList { get; set; } = new List<StatusEffect>();

    public Life life { get; private set; }
    public Energy energy { get; private set; }

    private RobotAnimation m_RobotAnimation;

    public Dictionary<Stats, int> CurrentRobotStats { get; private set; } = new Dictionary<Stats, int>();
    public Dictionary<Stats, int> DataRobotStats { get; private set; } = new Dictionary<Stats, int>();
    public List<CardData> RobotCards { get; private set; } = new List<CardData>();

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

        PrepareRobotStats();
    }

    private void PrepareRobotStats()
    {
        CurrentRobotStats[Stats.health] = m_Data.Health();
        DataRobotStats[Stats.health] = m_Data.Health();

        CurrentRobotStats[Stats.attack] = m_Data.Attack();
        DataRobotStats[Stats.attack] = m_Data.Attack();

        CurrentRobotStats[Stats.defence] = m_Data.Defense();
        DataRobotStats[Stats.defence] = m_Data.Defense();

        CurrentRobotStats[Stats.speed] = m_Data.Speed();
        DataRobotStats[Stats.speed] = m_Data.Speed();

        CurrentRobotStats[Stats.critChance] = m_Data.CritChance();
        DataRobotStats[Stats.critChance] = m_Data.CritChance();

        CurrentRobotStats[Stats.evasion] = m_Data.Evasion();
        DataRobotStats[Stats.evasion] = m_Data.Evasion();

        CurrentRobotStats[Stats.accuracy] = m_Data.Accuracy();
        DataRobotStats[Stats.accuracy] = m_Data.Accuracy();

        CurrentRobotStats[Stats.inteligence] = m_Data.Inteligence();
        DataRobotStats[Stats.inteligence] = m_Data.Inteligence();

        CurrentRobotStats[Stats.fireResistence] = m_Data.FireResistence();
        DataRobotStats[Stats.fireResistence] = m_Data.FireResistence();

        CurrentRobotStats[Stats.waterResistence] = m_Data.WaterResistence();
        DataRobotStats[Stats.waterResistence] = m_Data.WaterResistence();

        CurrentRobotStats[Stats.electricResistence] = m_Data.ElectricResistence();
        DataRobotStats[Stats.electricResistence] = m_Data.ElectricResistence();

        CurrentRobotStats[Stats.acidResistence] = m_Data.AcidResistence();
        DataRobotStats[Stats.acidResistence] = m_Data.AcidResistence();
    }

    private void Start()
    {
        RemoveAllBuffAndDebuff();

        Round.i.StartTurn.AddListener(() => {

            // remove this logic of here
            // because the buff and defuff
            // can be apply for more of one round

            //RemoveAllBuffAndDebuff();
            ActivateEarlyStatusEffects();
            Debug.LogWarning("Update Logic");
        });

        // Add Robot Attack Feedback To Event List
        Round.i.RobotAttack.AddListener((robot, target) =>
           RobotAttackFeedback(robot, target)
        );
    }

    /// <summary>Set Robot Behaviour After Attack Event Called</summary>
    private void RobotAttackFeedback(Robot robot, Robot target)
    {
        if (robot != this) return;
        m_RobotAnimation.PlayAnimation(Animations.action);
    }

    private void RemoveAllBuffAndDebuff()
    {
        ResetStat(Stats.attack);
        ResetStat(Stats.defence);
        ResetStat(Stats.speed);
    }

    public void ApplyStatChange(Stats stat, int value)
    {
        CurrentRobotStats[stat] = Mathf.Max(0, CurrentRobotStats[stat] + value);
        GameController.i.ShowAlertFeedback(value, Color.blue, m_iconSpawInLeft, stat, value);
    }

    public int StatDiff(Stats stat)
    {
        return CurrentRobotStats[stat] - DataRobotStats[stat]; 
    }

    public void ResetStat(Stats stat)
    {
        CurrentRobotStats[stat] = DataRobotStats[stat];
    }

    public RobotData Data()
    {
        return m_Data;
    }

    public void ApplyStatusEffect(StatusEffect newStatusEffect)
    {
        StatusList = newStatusEffect.UpdateStatusList(StatusList);
    }

    public bool ActivateEarlyStatusEffects()
    {
        foreach (var status in StatusList)
        {
            if (status.statusTrigger == StatusEffectTrigger.OnStartRound && status.ActivateStatusEffect(this))
            {
                StatusList.Remove(status);
            }
        }

        return true;
    }

    public async Task<bool> ActivateLateStatusEffects(int timeBetweenStatusEffects)
    {
        foreach (var status in StatusList)
        {
            if (status.statusTrigger == StatusEffectTrigger.OnEndRound && status.ActivateStatusEffect(this))
            {
                StatusList.Remove(status);
                await Task.Delay(timeBetweenStatusEffects);
            }
        }

        return true;
    }
}
