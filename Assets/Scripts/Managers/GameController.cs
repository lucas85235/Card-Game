using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public static GameController i;


    [Header("Setup")]
    [SerializeField] private float timeToPlay;
    [SerializeField] private Slider timeSlider;
    [SerializeField] private Robot[] robots;

    [Header("Alert")]
    [SerializeField] private GameObject alertText;
    [SerializeField] private RectTransform alertLeft;
    [SerializeField] private RectTransform alertRight;

    [Serializable]
    private struct Icon
    {
        public Sprite sprite;
        public Stats stat;
        public bool positive;
    }

    [SerializeField] private List<Icon> iconList;

    private Coroutine timeRoundCoroutine;
    private Dictionary<Stats, Dictionary<bool, Sprite>> m_IconDictionary = new Dictionary<Stats, Dictionary<bool, Sprite>>();

    private void Awake()
    {
        i = this;

        foreach (var icon in iconList)
        {
            if(!m_IconDictionary.ContainsKey(icon.stat))
            {
                m_IconDictionary[icon.stat] = new Dictionary<bool, Sprite>();
            }

            m_IconDictionary[icon.stat][icon.positive] = icon.sprite;
        }
    }

    void Start()
    {
        AudioManager.Instance.Play(AudiosList.gameplayMusic, isMusic: true);
        AudioManager.Instance.ChangeMusicVolumeWithLerp(1, 3f, startVolume: 0);

        StartCountdown();

        Round.i.EndTurn.AddListener(() => StartCountdown());
    }

    public void ShowAlertText(int value, bool left, Stats statToShow, Color textColor)
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
            .setOnUpdate((float value) =>
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
        timeSlider.gameObject.SetActive(true);
        timeRoundCoroutine = StartCoroutine(Countdown());
    }

    public void EndCountdown()
    {
        if (timeRoundCoroutine == null) return;

        timeSlider.gameObject.SetActive(false);
        StopCoroutine(timeRoundCoroutine);

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
}
