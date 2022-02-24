using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] protected bool useTimer = true;
    [SerializeField] protected float timeToPlay;
    [SerializeField] protected Slider timeSlider;

    [Header("Alert")]
    [SerializeField] protected GameObject alertText;
    [SerializeField] protected RectTransform alertLeft;
    [SerializeField] protected RectTransform alertRight;

    [Header("Icons")]
    [SerializeField] protected List<Icon> iconList;

    protected List<Robot> robots = new List<Robot>();
    protected Coroutine timeRoundCoroutine;
    protected Dictionary<Stats, Dictionary<bool, Sprite>> m_IconDictionary = new Dictionary<Stats, Dictionary<bool, Sprite>>();

    public static GameController i;

    protected virtual void Awake()
    {
        i = this;

        foreach (var icon in iconList)
        {
            if(!m_IconDictionary.ContainsKey(icon.stat))
                m_IconDictionary[icon.stat] = new Dictionary<bool, Sprite>();

            m_IconDictionary[icon.stat][icon.positive] = icon.sprite;
        }
    }

    protected virtual void Start()
    {
        robots.Add(Round.i.playerOne);
        robots.Add(Round.i.playerTwo);

        AudioManager.Instance.Play(AudiosList.gameplayMusic, isMusic: true);
        AudioManager.Instance.ChangeMusicVolumeWithLerp(1, 3f, startVolume: 0);

        timeSlider.gameObject.SetActive(useTimer);
    }

    public virtual void EndCountdown()
    {
        if (useTimer)
        {
            // Stop CountDown
        }
    }

    public virtual Robot GetTheOtherRobot(Robot emitterRobot)
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

    private void OnDestroy()
    {
        LeanTween.cancelAll();
    }

    [Serializable]
    protected struct Icon
    {
        public Sprite sprite;
        public Stats stat;
        public bool positive;
    }    
}
