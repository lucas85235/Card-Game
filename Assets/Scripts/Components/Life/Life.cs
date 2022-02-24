using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Linq;

[RequireComponent(typeof(Robot))]

public class Life : MonoBehaviour
{
    [Header("Death Setup")]
    public bool destroyAfterDeath = false;
    public bool isDead = false;

    [Header("Death Event")]
    public UnityEvent OnDeath;

    protected int m_maxLife;
    protected int m_currentLife;
    protected int m_currentShield;

    protected Robot m_robot;
    protected RobotAnimation m_RobotAnimation;

    protected virtual void Start()
    {
        m_robot = GetComponent<Robot>();
        TryGetComponent(out m_RobotAnimation);

        m_maxLife = m_robot.DataStats[Stats.health];
        m_currentLife = m_maxLife;
    }

    public virtual void AddLife(int increment)
    {
        m_currentLife += increment;
        LifeRules();
    }


    public virtual void TakeDamage(int decrement, params object[] objects) 
    { 
        m_currentLife -= decrement;
        LifeRules();
    }

    public virtual void AddShield(int shild) 
    {
        m_currentShield += shild;
    }
    
    public virtual void RemoveShild() 
    { 
        m_currentShield = 0;
    }

    protected virtual void LifeRules()
    {
        if (m_currentLife > m_maxLife)
        {
            m_currentLife = m_maxLife;
        }
        if (m_currentLife < 1)
        {
            AudioManager.Instance.Play(AudiosList.robotDeath);
            m_RobotAnimation.PlayAnimation(Animations.death);
            m_RobotAnimation.ResetToIdleAfterAnimation(false);
            DeathHandle();
        }
        else
        {
            m_RobotAnimation.PlayAnimation(Animations.hurt);
            AudioManager.Instance.Play(AudiosList.robotHurt);
        }
    }

    protected virtual void DeathHandle()
    {
        Debug.Log("Character is Death");

        if (destroyAfterDeath)
            Destroy(this.gameObject);

        OnDeath?.Invoke();
        isDead = true;
    }
}
