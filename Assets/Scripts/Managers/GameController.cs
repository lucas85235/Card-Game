using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("CHARACTERS")]
    public Robot playerOne;
    public Robot playerTwo;

    [Header("Setup")]
    public bool isMultiplayer;
    [SerializeField] private bool useTimer = true;
    [SerializeField] private float timeToPlay;
    [SerializeField] private Slider timeSlider;

    [Header("Alert")]
    [SerializeField] private GameObject alertText;
    [SerializeField] private RectTransform alertLeft;
    [SerializeField] private RectTransform alertRight;

    [Header("Icons")]
    [SerializeField] private List<Icon> iconList;

    private List<Robot> robots;
    private Coroutine timeRoundCoroutine;
    private Dictionary<Stats, Dictionary<bool, Sprite>> m_IconDictionary = new Dictionary<Stats, Dictionary<bool, Sprite>>();

    public static GameController i;

    protected virtual void Awake()
    {
        i = this;

        var round = FindObjectOfType<RoundLoop>();
        robots = new List<Robot>();

        robots.Add(playerOne);
        robots.Add(playerTwo);

        foreach (var icon in iconList)
        {
            if(!m_IconDictionary.ContainsKey(icon.stat))
                m_IconDictionary[icon.stat] = new Dictionary<bool, Sprite>();

            m_IconDictionary[icon.stat][icon.positive] = icon.sprite;
        }
    }

    protected virtual IEnumerator Start()
    {
        if (isMultiplayer)
        {
            yield return new WaitUntil( () => BasicConection.Instance.IsReady());
        }

        AudioManager.Instance.Play(AudiosList.gameplayMusic, isMusic: true);
        AudioManager.Instance.ChangeMusicVolumeWithLerp(1, 3f, startVolume: 0);

        if (useTimer)
        {
            Round.i.EndTurn.AddListener(() => StartCountdown());            
            StartCountdown();
        }

        timeSlider.gameObject.SetActive(useTimer);
    }

    public virtual void ShowAlertText(int value, bool left, Stats statToShow, Color textColor)
    {
        var referenceRect = left ? alertLeft : alertRight;
        int direction;

        var newAlert = Instantiate(alertText, referenceRect);
        newAlert.LeanScaleX(transform.localScale.x > 0 ? 1 : -1, 0);

        var imageObject = newAlert.transform.Find("AlertImage").gameObject;

        if (!m_IconDictionary.ContainsKey(statToShow) || !m_IconDictionary[statToShow].ContainsKey(value > 0))
        {
            Destroy(imageObject);
        }
        else 
        {
            imageObject.TryGetComponent(out Image imageComponent);
            imageComponent.sprite = m_IconDictionary[statToShow][value > 0];
        }

        if(value > 0)
        {
            AudioManager.Instance.Play(AudiosList.robotEffect);
            direction = 1;
        }
        else
        {
            AudioManager.Instance.Play(AudiosList.robotDeffect);
            direction = -1;
        }

        newAlert.transform.Find("AlertText").TryGetComponent(out TextMeshProUGUI textComponent);
        textComponent.text = value.ToString();
        textComponent.color = textColor;

        newAlert.TryGetComponent(out CanvasGroup textCGroup);
        LeanTween.value(1, 0, 2)
            .setOnUpdate( (float value) =>
            {
                textCGroup.alpha = value;
            });

        newAlert.TryGetComponent(out RectTransform textRect);
        textRect.LeanMoveLocalY(200 * direction, 2)
            .setOnComplete(() =>
            {
                Destroy(newAlert);
            });
    }

    public void StartCountdown()
    {
        if (!useTimer)
        {
            return;
        }

        timeSlider.gameObject.SetActive(true);
        timeRoundCoroutine = StartCoroutine(Countdown());
    }

    public void EndCountdown()
    {
        if (useTimer)
        {
            if (timeRoundCoroutine == null) return;

            timeSlider.gameObject.SetActive(false);
            StopCoroutine(timeRoundCoroutine);            
        }

        Round.i.StartTurn?.Invoke();
    }

    private IEnumerator Countdown()
    {
        float timeRemaining = timeToPlay;

        while (timeRemaining > 0)
        {
            yield return null;
            timeRemaining -= Time.deltaTime;
            timeSlider.value = timeRemaining / timeToPlay;
        }

        timeSlider.gameObject.SetActive(false);
        Round.i.StartTurn?.Invoke();
    }

    public Robot GetTheOtherRobot(Robot emitterRobot)
    {
        foreach (var robot in robots)
        {
            if(robot != emitterRobot)
            {
                return robot;
            }
        }

        return null;
    }

    private void OnDestroy()
    {
        LeanTween.cancelAll();
    }

    [Serializable]
    private struct Icon
    {
        public Sprite sprite;
        public Stats stat;
        public bool positive;
    }    
}
