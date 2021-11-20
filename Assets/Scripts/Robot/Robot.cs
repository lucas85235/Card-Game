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
    public RobotData data;
    public Transform selectedCardsConteriner;
    [SerializeField] private bool getFromDataManager;

    [Header("Rand Robot Data")]
    public bool randData = false;
    public RobotData[] datas;

    [Header("DATA")]
    [SerializeField] private int m_attack;
    [SerializeField] private int m_defense;
    [SerializeField] private int m_speed;
    [SerializeField] private int m_energy;

    [Header("CURRENT")]
    [SerializeField] private int m_currentAttack;
    [SerializeField] private int m_currentDefense;
    [SerializeField] private int m_currentSpeed;

    private bool m_iconSpawInLeft;

    public Life life { get; private set; }
    private RobotAnimation m_RobotAnimation;
    
    public int Attack() => m_currentAttack;
    public int Defense() => m_currentDefense;
    public int Speed() => m_currentSpeed;

    private void Awake()
    {
        if (randData && datas.Length > 1) data = datas[Random.Range(0, datas.Length)];

        life = GetComponent<Life>();
        TryGetComponent(out m_RobotAnimation);

        if (getFromDataManager) data = DataManager.Instance.GetCurrentRobot();
        m_RobotAnimation.ChangeRobotSprites(data);

        m_iconSpawInLeft = transform.localScale.x > 0;

        GetComponent<RobotAnimation>().ChangeRobotSprites(data);
    }

    private void Start()
    {
        LoadData();
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

    private void LoadData()
    {
        m_energy = data.Energy();
        m_attack = data.Attack();
        m_defense = data.Defense();
        m_speed = data.Speed();
    }

    private void RemoveAllBuffAndDebuff()
    {
        AttackReset();
        DefenseReset();
        SpeedReset();
    }

    // ATTACK

    public void AttackBuff(int buff)
    {
        m_currentAttack += buff;
        AudioManager.Instance.Play(AudiosList.robotEffect);
        GameController.i.ShowAlertText(buff, Color.blue, m_iconSpawInLeft, IconList.attackBuff);
    }

    public void AttackDebuff(int debuff)
    {
        m_currentAttack -= debuff;
        if (m_currentAttack < 1) m_currentAttack = 0;
        AudioManager.Instance.Play(AudiosList.robotDeffect);
        GameController.i.ShowAlertText(debuff, Color.black, m_iconSpawInLeft, IconList.attackDebuff, true);
    }

    public int AttackDiff() => m_currentAttack - m_attack;

    public void AttackReset()
    {
        m_currentAttack = m_attack;
    }

    // DEFENSE

    public void DefenseBuff(int buff)
    {
        m_currentDefense += buff;
        AudioManager.Instance.Play(AudiosList.robotEffect);
        GameController.i.ShowAlertText(buff, Color.blue, m_iconSpawInLeft, IconList.defenceBuff);
    }

    public void DefenseDebuff(int debuff)
    {
        // Debug.Log("DEBUFF DEF: -" + debuff);
        m_currentDefense -= debuff;
        if (m_currentAttack < 1) m_currentAttack = 0;
        AudioManager.Instance.Play(AudiosList.robotDeffect);
        GameController.i.ShowAlertText(debuff, Color.black, m_iconSpawInLeft, IconList.defenceDebuff, true);
    }

    public int DefenseDiff() => m_currentDefense - m_defense;

    public void DefenseReset()
    {
        m_currentDefense = m_defense;
    }

    // SPEED

    public void SpeedBuff(int buff)
    {
        m_currentSpeed += buff;
        AudioManager.Instance.Play(AudiosList.robotEffect);
    }

    public void SpeedDebuff(int debuff)
    {
        m_currentSpeed -= debuff;
        AudioManager.Instance.Play(AudiosList.robotDeffect);
    }

    public int SpeedDiff() => m_currentSpeed - m_speed;

    public void SpeedReset()
    {
        m_currentSpeed = m_speed;
    }
}
