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

    protected Robot m_robot;
    protected int m_maxLife;
    protected int m_currentLife;
    protected int m_currentShield;

    protected RobotAnimation m_RobotAnimation;
    protected Dictionary<Element, Stats> m_ElementToStats = new Dictionary<Element, Stats>();

    public bool HaveShield() => m_currentShield > 0;

    protected virtual void Start()
    {
        m_robot = GetComponent<Robot>();
        TryGetComponent(out m_RobotAnimation);

        if (lifeSlider == null) {
            Debug.LogError("lifeSlider is Null");
            return;
        }
        
        m_maxLife = m_robot.DataStats[Stats.health];
        lifeSlider.maxValue = m_maxLife;
        lifeSlider.value = m_maxLife;
        m_currentLife = (int) lifeSlider.value;

        UpdateLifeSlider();
        SetElementToStats();
    }

    protected virtual void SetElementToStats()
    {
        m_ElementToStats[Element.acid] = Stats.acidResistence;
        m_ElementToStats[Element.water] = Stats.waterResistence;
        m_ElementToStats[Element.fire] = Stats.fireResistence;
        m_ElementToStats[Element.electric] = Stats.electricResistence;
        m_ElementToStats[Element.normal] = Stats.defence;
    }

    public void AddLife(int increment)
    {
        var inteligence = m_robot.CurrentRobotStats[Stats.inteligence];
        increment = increment - inteligence;

        Debug.Log("Increment: " + increment);

        m_currentLife += increment;

        GameController.i.ShowAlertText(increment, transform.localScale.x > 0, Stats.health, Color.green);

        if (m_currentLife > m_maxLife)
            m_currentLife = m_maxLife;

        UpdateLifeSlider();
    }

    public virtual void TakeDamage(int decrement, AttackType type = AttackType.none, Element element = Element.normal, CardData usedCard = null, List<EffectSkill> skills = null)
    {
        bool ignoreShield = false;
        float hitChance = 1;
        float critChance = 0;

        if(usedCard != null)
        {
            ignoreShield = usedCard.Piercing;
            hitChance = Mathf.Clamp(1f + (m_robot.CurrentRobotStats[Stats.evasion] - GameController.i.GetTheOtherRobot(m_robot).CurrentRobotStats[Stats.accuracy]) / 100, 0f, 1f) - usedCard.MissChance;
            critChance = m_robot.CurrentRobotStats[Stats.critChance];
        }

        // Chance de errar o dano da carta
        if (Random.Range(0f, 1f) > hitChance)
        {
            Debug.Log("Misses attack");
            return;
        }

        // Chance de aplicar um critico no ataque
        if (Random.Range(0f, 1f) < critChance)
        {
            decrement += Mathf.FloorToInt(decrement / 2);
        }

        int resistence = 0;

        if (type != AttackType.none)
        {
            resistence = m_robot.CurrentRobotStats[m_ElementToStats[element]];
        }

        int damage = decrement - resistence;

        if (damage < 1) damage = 1;

        GameController.i.ShowAlertText(damage, transform.localScale.x > 0, Stats.health, Color.red);

        if (HaveShield() && !ignoreShield)
            damage = TakeDamageShield(damage);

        m_currentLife -= damage;

        if (usedCard != null)
        {
            if (skills != null)
            {
                foreach (var skill in skills)
                {
                    if (skill != null)
                    {
                        skill.ApplySkill(GameController.i.GetTheOtherRobot(m_robot), m_robot, damage, usedCard);
                    }
                }
            }

            var statusToRemove = new List<StatusEffect>();

            foreach (var status in m_robot.StatusList)
            {
                if (status != null && status.statusTrigger == StatusEffectTrigger.OnReceiveDamage && status.ActivateStatusEffect(m_robot, type))
                {
                    statusToRemove.Add(status);
                }
            }

            m_robot.StatusList = m_robot.StatusList.Except(statusToRemove).ToList();
        }

        LifeRules();
        UpdateLifeSlider();
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
        if (destroyAfterDeath) Destroy(this.gameObject);
        OnDeath?.Invoke();
        isDead = true;
    }

    protected void UpdateLifeSlider()
    {
        if (lifeSlider != null)
            lifeSlider.value = m_currentLife;

        if (lifeText != null)
            lifeText.text = m_currentLife + " / " + m_maxLife;
    }

    public virtual void AddShield(int shild)
    {
        // GameController.i.ShowAlertText(shild, Color.white, transform.localScale.x > 0);
        m_currentShield += shild;

        AudioManager.Instance.Play(AudiosList.robotEffect);
        shildSlider.gameObject.SetActive(true);
        shildSlider.maxValue = m_currentShield;
        shildSlider.value = m_currentLife;

        UpdateShildSlider();
    }

    public virtual void RemoveShild()
    {
        shildSlider.gameObject.SetActive(false);
        m_currentShield = 0;
    }

    protected int TakeDamageShield(int damage)
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

    protected void UpdateShildSlider()
    {
        if (shildSlider != null)
            shildSlider.value = m_currentShield;

        if (shildText != null)
            shildText.text = m_currentShield + " / " + shildSlider.maxValue;
    }
}
