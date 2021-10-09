using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    [Header("Set Character Data")]
    public RobotData data;

    private int m_attack;
    private int m_defense;
    private int m_speed;
    private int m_energy;

    private int currentAttack;
    private int currentDefense;
    private int currentSpeed;

    [Header("Set Character Data")]
    public Life life;

    void Start()
    {
        life = GetComponent<Life>();
        

        LoadData();
    }



    private void LoadData() 
    {
        m_energy = data.Energy();
        m_attack = data.Attack();
        m_defense = data.Defense();
        m_speed = data.Speed();
    }

    // ATTACK

    public void AttackBuff()
    {
        // Max buff is five
        if (currentAttack == (m_attack * 2)) return;
        int buff = (int) (m_attack * 0.2f);
        currentAttack += buff;
    }

    public void AttackDebuff()
    {
        // Max debuff is five
        if (currentAttack == (m_attack * 0.5f)) return;
        int debuff = (int) (m_attack * 0.1f);
        currentAttack -= debuff;
    }

    public void AttackReset()
    {
        currentAttack = m_attack;
    }

    // DEFENSE

    public void DefenseBuff()
    {
        // Max buff is five
        if (currentDefense == (m_defense * 2)) return;
        int buff = (int) (m_defense * 0.2f);
        currentDefense += buff;
    }

    public void DefenseDebuff()
    {
        // Max debuff is five
        if (currentDefense == (m_defense * 0.5f)) return;
        int debuff = (int) (m_defense * 0.1f);
        currentDefense -= debuff;
    }

    public void DefenseReset()
    {
        currentDefense = m_defense;
    }

    // SPEED

    public void SpeedBuff()
    {
        // Max buff is five
        if (currentSpeed == (m_speed * 2)) return;
        int buff = (int) (m_speed * 0.2f);
        currentSpeed += buff;
    }

    public void SpeedDebuff()
    {
        // Max debuff is five
        if (currentSpeed == (m_speed * 0.5f)) return;
        int debuff = (int) (m_speed * 0.1f);
        currentSpeed -= debuff;
    }

    public void SpeedReset()
    {
        currentSpeed = m_speed;
    }
}
