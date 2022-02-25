using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

[RequireComponent(typeof(Life))]
[RequireComponent(typeof(Energy))]

public class Robot : MonoBehaviour
{
    [Header("Set Character Data")]
    [SerializeField] protected RobotData m_Data;
    [SerializeField] protected bool getFromDataManager;
    [HideInInspector] public Transform selectedCardsConteriner;

    [Header("Rand Robot Data")]
    [SerializeField] protected bool randData = false;
    [SerializeField] protected RobotData[] datas;

    public Life life { get; private set; }
    public Energy energy { get; private set; }
    public Dictionary<Stats, int> CurrentRobotStats { get => currentRobotStats; }
    public Dictionary<Stats, int> DataStats { get => dataStats; }
    public List<CardData> CurrentCards { get; private set; }
    public List<StatusEffect> StatusList { get; set; } = new List<StatusEffect>();

    protected Dictionary<Stats, int> currentRobotStats = new Dictionary<Stats, int>();
    protected Dictionary<Stats, int> dataStats = new Dictionary<Stats, int>();
    protected RobotAnimation m_RobotAnimation;
    protected bool m_iconSpawInLeft;

    protected virtual void Awake()
    {
        life = GetComponent<Life>();
        energy = GetComponent<Energy>();
        m_RobotAnimation = GetComponent<RobotAnimation>();
        selectedCardsConteriner = GetComponent<DeckManager>().selectedConteriner;

        if (randData && datas.Length > 1)
        {
            m_Data = datas[Random.Range(0, datas.Length)];
        }

        if (getFromDataManager)
        {
            m_Data = DataManager.Instance.GetCurrentRobot();
        }

        m_RobotAnimation.ChangeRobotSprites(m_Data);
        m_iconSpawInLeft = transform.position.x < 0;

        SetStats();
        CurrentCards = m_Data.Cards();
    }

    protected virtual void Start()
    {
        RemoveAllBuffAndDebuff();

        Round.i.EndTurn.AddListener(() =>
            ActivateEarlyStatusEffects()
        );

        // Add Robot Attack Feedback To Event List
        Round.i.RobotAttack.AddListener((robot, target) =>
           RobotAttackFeedback(robot, target)
        );
    }

    /// <summary>Set Robot Behaviour After Attack Event Called</summary>
    protected virtual void RobotAttackFeedback(Robot robot, Robot target)
    {
        if (robot != this) return;
        m_RobotAnimation.PlayAnimation(Animations.action);
    }

    public virtual void RemoveCard(CardData cardToRemove)
    {
        CurrentCards.Remove(cardToRemove);
    }

    public virtual int StatDiff(Stats statToCompare)
    {
        return currentRobotStats[statToCompare] - DataStats[statToCompare];
    }

    protected virtual void StatReset(Stats statToReset)
    {
        currentRobotStats[statToReset] = DataStats[statToReset];
    }

    protected virtual void RemoveAllBuffAndDebuff()
    {
        StatReset(Stats.attack);
        StatReset(Stats.defence);
        StatReset(Stats.speed);
    }
    
    public virtual void ApplyStatChange(Stats statToChange, int value)
    {
        Debug.Log("statToChange: " + statToChange.ToString());
        Debug.Log("value: " + value.ToString());

        currentRobotStats[statToChange] += value;

        Debug.Log("currentRobotStats is null: " + (currentRobotStats == null));

        Debug.Log("VALUE: " + value);
        var textColor = value > 0 ? Color.blue : Color.red;

        Debug.Log("GameController.i is null: " + (GameController.i == null));

        GameController.i.ShowAlertText(value, m_iconSpawInLeft, statToChange, textColor);
    }

    public virtual void ApplyStatusEffect(StatusEffect newStatusEffect)
    {
        StatusList = newStatusEffect.UpdateStatusList(StatusList);
    }

    public virtual bool ActivateEarlyStatusEffects()
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

    public virtual async Task<bool> ActivateLateStatusEffects(int timeBetweenStatusEffects)
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

    protected void SetStats()
    {
        currentRobotStats[Stats.attack] = m_Data.Attack();
        dataStats[Stats.attack] = m_Data.Attack();

        currentRobotStats[Stats.defence] = m_Data.Defense();
        dataStats[Stats.defence] = m_Data.Defense();

        currentRobotStats[Stats.speed] = m_Data.Speed();
        dataStats[Stats.speed] = m_Data.Speed();

        currentRobotStats[Stats.evasion] = m_Data.Evasion();
        dataStats[Stats.evasion] = m_Data.Evasion();

        currentRobotStats[Stats.accuracy] = m_Data.Accuracy();
        dataStats[Stats.accuracy] = m_Data.Accuracy();

        currentRobotStats[Stats.inteligence] = m_Data.Inteligence();
        dataStats[Stats.inteligence] = m_Data.Inteligence();

        currentRobotStats[Stats.health] = m_Data.Health();
        dataStats[Stats.health] = m_Data.Health();

        currentRobotStats[Stats.critChance] = m_Data.CritChance();
        dataStats[Stats.critChance] = m_Data.CritChance();

        currentRobotStats[Stats.fireResistence] = m_Data.FireResistence();
        dataStats[Stats.fireResistence] = m_Data.FireResistence();

        currentRobotStats[Stats.waterResistence] = m_Data.WaterResistence();
        dataStats[Stats.waterResistence] = m_Data.WaterResistence();

        currentRobotStats[Stats.acidResistence] = m_Data.AcidResistence();
        dataStats[Stats.acidResistence] = m_Data.AcidResistence();

        currentRobotStats[Stats.electricResistence] = m_Data.ElectricResistence();
        dataStats[Stats.electricResistence] = m_Data.ElectricResistence();
    }
}
