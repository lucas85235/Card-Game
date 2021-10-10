using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Life))]
[RequireComponent(typeof(Energy))]

public class Robot : MonoBehaviour
{
    [Header("Set Character Data")]
    public RobotData data;

    [Header("DATA")]
    [SerializeField] private int m_attack;
    [SerializeField] private int m_defense;
    [SerializeField] private int m_speed;
    [SerializeField] private int m_energy;

    [Header("CURRENT")]
    [SerializeField] private int m_currentAttack;
    [SerializeField] private int m_currentDefense;
    [SerializeField] private int m_currentSpeed;

    public int Attack() => m_currentAttack;
    public int Defense() => m_currentDefense;
    public int Speed() => m_currentSpeed;

    private Life m_life;
    private Energy m_energyCount;
    private List<CardImage> m_roundCards;

    private RobotAnimation m_RobotAnimation;

    public Life life { get => m_life; }
    public Energy energy { get => m_energyCount; }

    public Transform selectedConteriner { get; set; }

    private void Awake()
    {
        m_life = GetComponent<Life>();
        m_energyCount = GetComponent<Energy>();
        TryGetComponent(out m_RobotAnimation);
    }

    private void Start()
    {
        LoadData();
        RemoveAllBuffAndDebuff();

        GameController.i.OnStartTurn.AddListener(() => RemoveAllBuffAndDebuff());
    }

    private void LoadData()
    {
        m_energy = data.Energy();
        m_attack = data.Attack();
        m_defense = data.Defense();
        m_speed = data.Speed();
    }

    // Pega as cartas do robo atual e aplica o dano ao proximo robo
    public IEnumerator UseRoundCards(Robot enemy, Action<bool> onEnd)
    {
        // Debug.Log("ROBOT: " + energy);

        m_roundCards = new List<CardImage>();

        for (int i = 0; i < selectedConteriner.childCount; i++)
        {
            var data = selectedConteriner.GetChild(i).GetComponent<CardImage>();
            m_roundCards.Add(data);
        }

        foreach (var card in m_roundCards)
        {
            card.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            m_RobotAnimation.PlayAnimation(Animations.action);

            yield return new WaitForSeconds(0.8f);
            
            card.UseEffects(this, enemy);
            card.gameObject.SetActive(false);
        }

        if (onEnd != null)
            onEnd(false);
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
    }

    public void AttackDebuff(int debuff)
    {
        m_currentAttack -= debuff;
        AudioManager.Instance.Play(AudiosList.robotDeffect);
    }

    public void AttackReset()
    {
        m_currentAttack = m_attack;
    }

    // DEFENSE

    public void DefenseBuff(int buff)
    {
        m_currentDefense += buff;
        AudioManager.Instance.Play(AudiosList.robotEffect);
    }

    public void DefenseDebuff(int debuff)
    {
        Debug.Log("DEBUFF DEF: -" + debuff);
        m_currentDefense -= debuff;
        AudioManager.Instance.Play(AudiosList.robotDeffect);
    }

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

    public void SpeedReset()
    {
        m_currentSpeed = m_speed;
    }
}
