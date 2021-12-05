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

    private bool m_iconSpawInLeft;

    public List<StatusEffect> StatusList { get; set; } = new List<StatusEffect>();

    public Life life { get; private set; }
    private RobotAnimation m_RobotAnimation;
    
    public int Attack() => m_currentAttack;
    public int Defense() => m_currentDefense;
    public int Speed() => m_currentSpeed;

    private void Awake()
    {
        if (randData && datas.Length > 1) m_Data = datas[Random.Range(0, datas.Length)];

        life = GetComponent<Life>();
        TryGetComponent(out m_RobotAnimation);

        if (getFromDataManager) m_Data = DataManager.Instance.GetCurrentRobot();
        m_RobotAnimation.ChangeRobotSprites(m_Data);

        m_iconSpawInLeft = transform.localScale.x > 0;

        GetComponent<RobotAnimation>().ChangeRobotSprites(m_Data);
    }

    private void Start()
    {
        RemoveAllBuffAndDebuff();

        Round.i.StartTurn.AddListener(() => {

            // remove this logic of here
            // because the buff and defuff
            // can be apply for more of one round

            RemoveAllBuffAndDebuff();
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
        AttackReset();
        DefenseReset();
        SpeedReset();
    }

    // ATTACK

    public void ApplyAttackChange(int value)
    {
        m_currentAttack += value;

        if(value > 0)
        {
            AudioManager.Instance.Play(AudiosList.robotEffect);
            GameController.i.ShowAlertText(value, Color.blue, m_iconSpawInLeft, IconList.attackBuff);
        }
        else
        {
            if (m_currentAttack < 1)
            {
                m_currentAttack = 0;
            }
            AudioManager.Instance.Play(AudiosList.robotDeffect);
            GameController.i.ShowAlertText(value, Color.black, m_iconSpawInLeft, IconList.attackDebuff, true);
        }
    }

    public int AttackDiff() => m_currentAttack - m_Data.Attack();

    public void AttackReset()
    {
        m_currentAttack = m_Data.Attack();
    }

    // DEFENSE

    public void ApplyDefenceChange(int value)
    {
        m_currentDefense += value;

        if (value > 0)
        {
            AudioManager.Instance.Play(AudiosList.robotEffect);
            GameController.i.ShowAlertText(value, Color.blue, m_iconSpawInLeft, IconList.defenceBuff);
        }
        else
        {
            if (m_currentAttack < 1)
            {
                m_currentAttack = 0;
            }
            AudioManager.Instance.Play(AudiosList.robotDeffect);
            GameController.i.ShowAlertText(value, Color.black, m_iconSpawInLeft, IconList.defenceDebuff, true);
        }
    }

    public int DefenseDiff() => m_currentDefense - m_Data.Defense();

    public void DefenseReset()
    {
        m_currentDefense = m_Data.Defense();
    }

    // SPEED

    public void ApplySpeedChange(int value)
    {
        m_currentSpeed += value;

        if(value > 0)
        {
            AudioManager.Instance.Play(AudiosList.robotEffect);
            GameController.i.ShowAlertText(value, Color.blue, m_iconSpawInLeft, IconList.speedBuff);
        }
        else
        {

            AudioManager.Instance.Play(AudiosList.robotDeffect);
            GameController.i.ShowAlertText(value, Color.black, m_iconSpawInLeft, IconList.speedDebuff, true);
        }
    }

    public int SpeedDiff() => m_currentSpeed - m_Data.Speed();

    public void SpeedReset()
    {
        m_currentSpeed = m_Data.Speed();
    }

    public RobotData Data()
    {
        return m_Data;
    }

    public void ApplyStatusEffect(StatusEffect newStatusEffect)
    {
        StatusList = newStatusEffect.UpdateStatusList(StatusList);
    }

    public void ActivateLateStatusEffects()
    {
        foreach (var status in StatusList)
        {
            if (status.statusTrigger == StatusEffectTrigger.OnEndRound && status.ActivateStatusEffect(this))
            {
                StatusList.Remove(status);
            }
        }
    }
}
