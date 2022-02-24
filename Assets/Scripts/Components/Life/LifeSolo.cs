using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.Linq;

[RequireComponent(typeof(Robot))]

public class LifeSolo : Life
{
    [Header("Life Settings")]
    public Slider lifeSlider;
    public TextMeshProUGUI lifeText;

    [Header("Shild Settings")]
    public Slider shildSlider;
    public TextMeshProUGUI shildText;

    private Dictionary<Element, Stats> m_ElementToStats = new Dictionary<Element, Stats>();

    protected override void Start()
    {
        base.Start();

        if (lifeSlider == null) {
            Debug.LogError("lifeSlider is Null");
            return;
        }
        
        lifeSlider.maxValue = m_maxLife;
        lifeSlider.value = m_maxLife;

        UpdateLifeSlider();
        SetElementToStats();
    }

    private void SetElementToStats()
    {
        m_ElementToStats[Element.acid] = Stats.acidResistence;
        m_ElementToStats[Element.water] = Stats.waterResistence;
        m_ElementToStats[Element.fire] = Stats.fireResistence;
        m_ElementToStats[Element.electric] = Stats.electricResistence;
        m_ElementToStats[Element.normal] = Stats.defence;
    }

    public override void AddLife(int increment)
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

    public override void TakeDamage(int decrement, params object[] objects)
    {
        AttackType type = (AttackType) (objects[0] != null ? objects[0] : AttackType.none);
        Element element = (Element) (objects[1] != null ? objects[1] : Element.normal);
        CardData usedCard = (CardData) (objects[2] != null ? objects[2] : null);
        List<EffectSkill> skills = (List<EffectSkill>) (objects[3] != null ? objects[3] : null);

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

        if (m_currentShield > 0 && !ignoreShield)
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

    private void UpdateLifeSlider()
    {
        if (lifeSlider != null)
            lifeSlider.value = m_currentLife;

        if (lifeText != null)
            lifeText.text = m_currentLife + " / " + m_maxLife;
    }

    public override void AddShield(int shild)
    {
        // GameController.i.ShowAlertText(shild, Color.white, transform.localScale.x > 0);
        m_currentShield += shild;

        AudioManager.Instance.Play(AudiosList.robotEffect);
        shildSlider.gameObject.SetActive(true);
        shildSlider.maxValue = m_currentShield;
        shildSlider.value = m_currentLife;

        UpdateShildSlider();
    }

    public override void RemoveShild()
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
