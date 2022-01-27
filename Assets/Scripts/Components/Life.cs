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
    public bool isDead = false;

    [Header("Death Event")]
    public UnityEvent OnDeath;

    private Robot m_robot;
    private int m_maxLife;
    private int m_currentLife;
    private int m_currentShield;

    private RobotAnimation m_RobotAnimation;

    public bool HaveShield() => m_currentShield > 0;

    void Start()
    {
        m_robot = GetComponent<Robot>();
        TryGetComponent(out m_RobotAnimation);

        if (lifeSlider == null) {
            Debug.LogError("lifeSlider is Null");
            return;
        }
        
        m_maxLife = m_robot.Data().Health();
        lifeSlider.maxValue = m_maxLife;
        lifeSlider.value = m_maxLife;
        m_currentLife = (int) lifeSlider.value;

        UpdateLifeSlider();
    }

    public void AddLife(int increment)
    {
        if(increment < 0)
        {
            TakeDamage(-increment, AttackType.none);
            return;
        }

        m_currentLife += increment;

        LifeRules();
        UpdateLifeSlider();
    }

    public void TakeDamage(int decrement, AttackType type, CardData usedCard = null, List<EffectSkill> skills = null)
    {
        bool ignoreShield = false;
        bool miss = false;
        float hitChance = 1;
        float critChance = 0;

        if(usedCard != null)
        {
            ignoreShield = usedCard.Piercing;
            hitChance -= Mathf.Clamp(1f + m_robot.Accuracy() - GameController.i.GetTheOtherRobot(m_robot).Evasion(), 0f, 1f) - usedCard.MissChance;
            critChance = m_robot.CritChance();
        }

        //Chance de errar o dano da carta
        if (Random.Range(0f, 1f) > hitChance)
        {
            decrement = 0;
            miss = true;
        }

        //Chance de aplicar um crítico no ataque
        else if (Random.Range(0f, 1f) < critChance)
        {
            decrement += Mathf.FloorToInt(decrement / 2);
        }

        if (miss)
        {
            Debug.Log("Misses attack");
            return;
        }

        else
        {
            int damage = decrement;

            if (HaveShield() && !ignoreShield)
                damage = TakeDamageShield(decrement);

            m_currentLife -= damage;

            if(usedCard != null)
            {
                if (skills != null)
                {
                    foreach (var skill in skills)
                    {
                        if (skill != null)
                        {
                            skill.ApplySkill(m_robot, GameController.i.GetTheOtherRobot(m_robot), damage, usedCard, skills);
                        }
                    }
                }

                foreach (var status in m_robot.StatusList)
                {
                    if (status != null && status.statusTrigger == StatusEffectTrigger.OnReceiveDamage && status.ActivateStatusEffect(m_robot, type))
                    {
                        m_robot.StatusList.Remove(status);
                    }
                }
            }

            GameController.i.ShowAlertText(decrement, Color.red, transform.localScale.x > 0);

            LifeRules();
            UpdateLifeSlider();
        }
    }

    private void DeathHandle()
    {
        Debug.Log("Character is Death");
        if (destroyAfterDeath) Destroy(this.gameObject);
        OnDeath?.Invoke();
        isDead = true;
    }

    private void LifeRules()
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

    private void UpdateLifeSlider()
    {
        if (lifeSlider != null)
            lifeSlider.value = m_currentLife;

        if (lifeText != null)
            lifeText.text = m_currentLife + " / " + m_maxLife;
    }

    public void AddShield(int shild)
    {
        // GameController.i.ShowAlertText(shild, Color.white, transform.localScale.x > 0);
        m_currentShield += shild;

        AudioManager.Instance.Play(AudiosList.robotEffect);
        shildSlider.gameObject.SetActive(true);
        shildSlider.maxValue = m_currentShield;
        shildSlider.value = m_currentLife;

        UpdateShildSlider();
    }

    public void RemoveShild()
    {
        shildSlider.gameObject.SetActive(false);
        m_currentShield = 0;
    }

    private int TakeDamageShield(int damage)
    {
        // Debug.Log("Damage: " + damage);
        
        m_currentShield -= damage;
        UpdateShildSlider();

        if (m_currentShield <= 0)
        {
            shildSlider.gameObject.SetActive(false);
            Debug.Log("Damage diff: " + m_currentShield * -1);
            return m_currentShield * -1;
        }
        
        return 0;
    }

    private void UpdateShildSlider()
    {
        if (shildSlider != null)
            shildSlider.value = m_currentShield;

        if (shildText != null)
            shildText.text = m_currentShield + " / " + shildSlider.maxValue;
    }
}
