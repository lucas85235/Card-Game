using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private float timeToPlay;
    [SerializeField] private Slider timeSlider;
    [SerializeField] private Robot[] robots;

    [Header("Alert")]
    [SerializeField] private GameObject alertText;
    [SerializeField] private RectTransform alertLeft;
    [SerializeField] private RectTransform alertRight;
    [SerializeField] private List<IconStruct> icons;

    private Dictionary<IconList, Sprite> m_IconDictionary;
    private Coroutine timeRoundCoroutine;

    public static GameController i;

    private void Awake()
    {
        i = this;

        m_IconDictionary = new Dictionary<IconList, Sprite>();

        foreach (var icon in icons)
            m_IconDictionary[icon.name] = icon.sprite;
    }

    void Start()
    {

        AudioManager.Instance.Play(AudiosList.gameplayMusic, isMusic: true);
        AudioManager.Instance.ChangeMusicVolumeWithLerp(1, 3f, startVolume: 0);

        StartCountdown();

        Round.i.EndTurn.AddListener(() => StartCountdown());
    }

    public void ShowAlertText(int decrement, Color textColor, bool left, IconList iconName = IconList.none, bool debuff = false)
    {
        var referenceRect = left ? alertLeft : alertRight;

        var newAlert = Instantiate(alertText, referenceRect);
        newAlert.LeanScaleX(transform.localScale.x > 0 ? 1 : -1, 0);

        var imageObject = newAlert.transform.Find("AlertImage").gameObject;

        if (iconName == IconList.none) Destroy(imageObject);
        else
        {
            imageObject.TryGetComponent(out Image imageComponent);
            imageComponent.sprite = m_IconDictionary[iconName];
        }

        newAlert.transform.Find("AlertText").TryGetComponent(out TextMeshProUGUI textComponent);
        textComponent.text = decrement.ToString();
        textComponent.color = textColor;

        newAlert.TryGetComponent(out CanvasGroup textCGroup);
        LeanTween.value(1, 0, 2)
            .setOnUpdate((float value) =>
            {
                textCGroup.alpha = value;
            });

        var direction = debuff ? -1 : 1;

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

    [Serializable]
    private struct IconStruct
    {
        public IconList name;
        public Sprite sprite;
    }
}
