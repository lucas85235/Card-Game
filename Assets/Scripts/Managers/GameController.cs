using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private bool useTimer = true;
    [SerializeField] private float timeToPlay;
    [SerializeField] private Image timeImage;
    [SerializeField] private Button endTurnButton;

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

    private void Awake()
    {
        i = this;

        var round = FindObjectOfType<RoundLoop>();
        robots = new List<Robot>();

        robots.Add(round.playerOne);
        robots.Add(round.playerTwo);

        foreach (var icon in iconList)
        {
            if(!m_IconDictionary.ContainsKey(icon.stat))
                m_IconDictionary[icon.stat] = new Dictionary<bool, Sprite>();

            m_IconDictionary[icon.stat][icon.positive] = icon.sprite;
        }
    }

    void Start()
    {
        AudioManager.Instance.Play(AudiosList.gameplayMusic, isMusic: true);
        AudioManager.Instance.ChangeMusicVolumeWithLerp(1, 3f, startVolume: 0);

        if (useTimer)
        {
            Round.i.EndTurn.AddListener(() => StartCountdown());            
            StartCountdown();
        }

        timeImage.gameObject.SetActive(useTimer);
        endTurnButton.interactable = useTimer;
    }

    public void ShowAlertText(int value, bool left, Stats statToShow, Color textColor, string beforeText = "")
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
        textComponent.text = beforeText + value.ToString();
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

    public void ShowMessageText(bool left, Color textColor, string messageToShow)
    {
        var referenceRect = left ? alertLeft : alertRight;
        int direction;

        var newAlert = Instantiate(alertText, referenceRect);
        newAlert.LeanScaleX(transform.localScale.x > 0 ? 1 : -1, 0);

        var imageObject = newAlert.transform.Find("AlertImage").gameObject;
        Destroy(imageObject);

        AudioManager.Instance.Play(AudiosList.robotDeffect);
        direction = 1;

        newAlert.transform.Find("AlertText").TryGetComponent(out TextMeshProUGUI textComponent);
        textComponent.text = messageToShow;
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
        if (!useTimer)
        {
            return;
        }

        timeImage.gameObject.SetActive(true);
        endTurnButton.interactable = true;

        timeRoundCoroutine = StartCoroutine(Countdown());
    }

    public void EndCountdown()
    {
        if (useTimer)
        {
            if (timeRoundCoroutine == null) return;

            timeImage.gameObject.SetActive(false);
            endTurnButton.interactable = false;

            StopCoroutine(timeRoundCoroutine);            
        }

        Round.i.StartTurn?.Invoke();
    }

    private IEnumerator Countdown()
    {
        float timeRemaining = 0;

        while (timeRemaining < timeToPlay)
        {
            yield return null;
            timeRemaining += Time.deltaTime;
            timeImage.fillAmount = 0.5f + (timeRemaining / timeToPlay) / 2;
        }

        timeImage.gameObject.SetActive(false);
        endTurnButton.interactable = false;

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
