using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class RobotSingleplayer : Robot
{
    [Header("Custom Settings")]
    public Transform selectedCardsConteriner;
    [SerializeField] private bool getFromDataManager;

    [Header("Rand Robot Data")]
    public bool randData = false;
    public RobotData[] datas;

    public Life life { get; protected set; }
    public Energy energy { get; protected set; }

    private void Awake()
    {
        if (randData && datas.Length > 1)
        {
            m_Data = datas[Random.Range(0, datas.Length)];
        }

        life = GetComponent<Life>();
        energy = GetComponent<Energy>();

        TryGetComponent(out m_RobotAnimation);

        if (getFromDataManager && DataManager.Instance != null)
        {
            m_Data = DataManager.Instance.GetCurrentRobot();
        }

        m_RobotAnimation.ChangeRobotSprites(m_Data);
        m_iconSpawInLeft = transform.localScale.x > 0;

        CurrentCards = m_Data.Cards();
        SetStats();

        RemoveAllBuffAndDebuff();
    }

    private void Start()
    {
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
}
