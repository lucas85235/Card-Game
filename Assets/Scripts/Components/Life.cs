using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Life : MonoBehaviour
{
    [Header("Life Setup")]
    public bool startInit = true;

    [Header("Life UI")]
    [SerializeField] private Slider lifeSlider;
    [SerializeField] private TextMeshProUGUI lifeText;

    [Header("Shild UI")]
    [SerializeField] private Slider shildSlider;
    [SerializeField] private TextMeshProUGUI shildText;

    [Header("Death Setup")]
    [SerializeField] private bool destroyAfterDeath = true;
    [SerializeField] private bool isDead = false;

    [Header("Death Event")]
    public UnityEvent OnDeath;
    public UnityEvent OnLifeUpdate;

    private Robot m_robot;
    private int m_maxLife;
    private int m_currentLife;
    private int m_currentShield;

    public int CurrentLife 
    { 
        get => m_currentLife; 
        set
        {
            m_currentLife = value;
            OnLifeUpdate?.Invoke();
        }
    }
    public bool IsDead { get => isDead; }

    private RobotAnimation m_RobotAnimation;
    private Dictionary<Element, Stats> m_ElementToStats = new Dictionary<Element, Stats>();

    public bool HaveShield() => m_currentShield > 0;

    private void Awake()
    {
        m_robot = GetComponent<Robot>();
        TryGetComponent(out m_RobotAnimation);

        if (lifeSlider == null) {
            Debug.LogError("lifeSlider is Null");
            return;
        }

        OnDeath.AddListener(() => isDead = true);
    }

    private void Start()
    {
        if (startInit) SetLife(m_robot.DataStats[Stats.health]);
    }

    private void SetElementToStats()
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

        CurrentLife += increment;

        GameController.i.ShowAlertText(increment, transform.localScale.x > 0, Stats.health, Color.green);

        if (CurrentLife > m_maxLife)
            CurrentLife = m_maxLife;

        UpdateLifeSlider();
    }

    public void SetLife(int life)
    {
        m_maxLife = m_robot.DataStats[Stats.health];
        m_currentLife = life;
        lifeSlider.maxValue = m_maxLife;
        lifeSlider.value = m_currentLife;
        
        CurrentLife = (int) lifeSlider.value;

        UpdateLifeSlider();
        SetElementToStats();
    }

    public void TakeDamage(int decrement, AttackType type = AttackType.none, Element element = Element.normal, CardData usedCard = null, List<EffectSkill> skills = null)
    {
        bool ignoreShield = false;
        float hitChance = 1;
        float critChance = 0;
        string addicionalMessage = "";

        if(usedCard != null)
        {
            ignoreShield = usedCard.Piercing;
            hitChance = Mathf.Clamp(1f + (m_robot.CurrentRobotStats[Stats.evasion] - GameController.i.GetTheOtherRobot(m_robot).CurrentRobotStats[Stats.accuracy]) / 100, 0f, 1f) - usedCard.MissChance;
            critChance = m_robot.CurrentRobotStats[Stats.critChance] / 100;
        }

        // Chance de errar o dano da carta
        if (Random.Range(0f, 1f) >= hitChance)
        {
            GameController.i.ShowMessageText(transform.localScale.x > 0, Color.blue, "Miss!");
            return;
        }

        // Chance de aplicar um critico no ataque
        if (Random.Range(0f, 1f) <= critChance)
        {
            decrement += Mathf.FloorToInt(decrement / 2);
            addicionalMessage = "Crit! ";
        }

        int resistence = 0;

        if (type != AttackType.none)
        {
            resistence = m_robot.CurrentRobotStats[m_ElementToStats[element]];
        }

        int damage = decrement - resistence;

        if (damage < 1) damage = 1;

        GameController.i.ShowAlertText(damage, transform.localScale.x > 0, Stats.health, Color.red, addicionalMessage);

        if (HaveShield() && !ignoreShield)
            damage = TakeDamageShield(damage);

        CurrentLife -= damage;

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

        UpdateLifeSlider();
        LifeRules();
    }

    private void DeathHandle()
    {
        Debug.Log("Character is Death");

        OnDeath?.Invoke();
        if (destroyAfterDeath) Destroy(this.gameObject);
    }

    private void LifeRules()
    {
        if (CurrentLife > m_maxLife)
            CurrentLife = m_maxLife;

        if (CurrentLife < 1)
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
        shildSlider.value = CurrentLife;

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
