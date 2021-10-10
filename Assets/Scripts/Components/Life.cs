using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(Robot))]

public class Life : MonoBehaviour
{
    [Header("Life Settings")]
    public Slider lifeSlider;
    public TextMeshProUGUI lifeText;

    [Header("Shild Settings")]
    public Slider shildSlider;
    public TextMeshProUGUI shildText;

    [Header("Death Setup")]
    public bool destroyAfterDeath = true;
    public bool isDeath = false;

    [Header("Death Event")]
    public UnityEvent OnDeath;

    private Robot m_robot;
    private int m_maxLife;
    private int m_currentLife;
    private int m_currentShild;

    private RobotAnimation m_RobotAnimation;

    public bool HaveShild() => m_currentShild > 0;

    void Start()
    {
        m_robot = GetComponent<Robot>();
        TryGetComponent(out m_RobotAnimation);

        if (lifeSlider == null) {
            Debug.LogError("lifeSlider is Null");
            return;
        }
        
        m_maxLife = m_robot.data.Health();
        lifeSlider.maxValue = m_maxLife;
        lifeSlider.value = m_maxLife;
        m_currentLife = (int) lifeSlider.value;

        UpdateLifeSlider();
    }

    public void AddLife(int increment)
    {
        m_currentLife += increment;

        LifeRules();
        UpdateLifeSlider();
    }

    public void TakeDamage(int decrement)
    {
        int damage = decrement;
        m_RobotAnimation.PlayAnimation(Animations.hurt);

        if (HaveShild())
            damage = TakeDamageShild(decrement);

        m_currentLife -= damage;

        LifeRules();
        UpdateLifeSlider();
    }

    private void DeathHandle()
    {
        Debug.Log("Character is Death");
        if (destroyAfterDeath) Destroy(this.gameObject);
        OnDeath?.Invoke();
        isDeath = true;
    }

    private void LifeRules()
    {
        if (m_currentLife > m_maxLife)
        {
            m_currentLife = m_maxLife;
        }
        if (m_currentLife < 1)
        {
            m_RobotAnimation.PlayAnimation(Animations.death);
            m_RobotAnimation.ResetToIdleAfterAnimation(false);
            DeathHandle();
        } 
    }    

    private void UpdateLifeSlider()
    {
        if (lifeSlider != null)
            lifeSlider.value = m_currentLife;

        if (lifeText != null)
            lifeText.text = m_currentLife + " / " + m_maxLife;
    }

    public void AddShild(int shild)
    {
        m_currentShild += shild;
        
        shildSlider.gameObject.SetActive(true);
        shildSlider.maxValue = m_currentShild;
        shildSlider.value = m_currentLife;

        UpdateShildSlider();
    }

    public void RemoveShild()
    {
        shildSlider.gameObject.SetActive(false);
        m_currentShild = 0;
    }

    private int TakeDamageShild(int damage)
    {
        Debug.Log("Damage: " + damage);
        
        m_currentShild -= damage;
        UpdateShildSlider();

        if (m_currentShild <= 0)
        {
            shildSlider.gameObject.SetActive(false);
            Debug.Log("Damage diff: " + m_currentShild * -1);
            return m_currentShild * -1;
        }
        
        return 0;
    }

    private void UpdateShildSlider()
    {
        if (shildSlider != null)
            shildSlider.value = m_currentShild;

        if (shildText != null)
            shildText.text = m_currentShild + " / " + shildSlider.maxValue;
    }
}
