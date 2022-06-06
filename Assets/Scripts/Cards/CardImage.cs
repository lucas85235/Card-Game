using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardImage : MonoBehaviour
{
    [Header("Setup")]
    public TextMeshProUGUI title;
    public TextMeshProUGUI energy;
    public TextMeshProUGUI description;
    public GameObject selectedFeedback;
    public Image image;

    [Header("Setup")]
    public Transform selectConteriner;
    public Energy energyCount;

    [Header("Debug")]
    public bool selected = false;

    public Action OnSelect;
    public Action OnDeselect;

    public CardData Data { get; set; }
    public Robot ConnectedRobot { get; set; }

    private bool m_canSelect = true;
    private bool m_canInteract = true;

    private RectTransform m_CardRectTransform;
    private Vector2 defaultDeltaSize;

    public void SetCanSelect(bool state) => m_canSelect = state;

    private void Awake()
    {
        TryGetComponent(out m_CardRectTransform);
        defaultDeltaSize = m_CardRectTransform.sizeDelta;
    }

    void Start()
    {
        if (Data == null) return;

        image.sprite = Data.Sprite();
        energy.text = Data.Energy().ToString();

        LanguageManager.Instance.OnChangeLanguage += UpdateText;
        UpdateText();

        if (Round.i != null)
        {
            Round.i.EndTurn.AddListener(() => OnStartTurn());
            Round.i.StartTurn.AddListener(() => m_canSelect = false);            
        }
    }

    private void OnEnable()
    {
        if (selected && m_CardRectTransform.sizeDelta == defaultDeltaSize)
        {
            m_CardRectTransform.sizeDelta = (defaultDeltaSize * 1.2f);
        }    
    }

    private void UpdateText()
    {
        title.text = LanguageManager.Instance.GetKeyValue(Data.TitleKey());
        description.text = LanguageManager.Instance.GetKeyValue(Data.DescriptionKey());
    }

    public void UseEffect()
    {
        foreach (var effect in Data.Effects())
        {
            if (GameController.i != null)
                effect.UseEffect(ConnectedRobot, GameController.i.GetTheOtherRobot(ConnectedRobot), Data);

            else
                effect.UseEffect(ConnectedRobot, Multiplayer.GameManager.Instance.GetTheOtherRobot(ConnectedRobot), Data);
        }
    }

    public void OnPointerEnter()
    {
        if (!m_canSelect || !m_canInteract || selected)
        {
            return;
        }

        var scaleOrientation = !selected ? 1 : -1;
        m_CardRectTransform.sizeDelta = (defaultDeltaSize * 1.5f);
    }

    public void OnPointerExit()
    {
        if (!m_canSelect || !m_canInteract || selected)
        {
            return;
        }

        m_CardRectTransform.sizeDelta = defaultDeltaSize;
    }

    public void OnClick()
    {
        // Verifica se está no turno
        if (!m_canSelect) return;

        if (!selected)
        {
            Select();
        }
        else Deselect();
    }

    public void Select()
    {        
        int decreaseEnergy = energyCount.EnergyRoundAmount - Data.Energy();

        if (energyCount.EnergyRoundAmount >= Data.Energy() &&
            decreaseEnergy >= 0 && !selected)
        {
            AudioManager.Instance.Play(AudiosList.cardPush);

            selected = true;
            selectedFeedback.gameObject.SetActive(true);

            if (energyCount != null)
                energyCount.UseRoundEnergy(-Data.Energy());

            if (m_CardRectTransform != null)
                m_CardRectTransform.sizeDelta = (defaultDeltaSize * 1.2f);

            OnSelect?.Invoke();
        }
    }

    public void Deselect()
    {
        if (selected)
        {
            AudioManager.Instance.Play(AudiosList.cardPush);

            selected = false;
            selectedFeedback.gameObject.SetActive(false);

            energyCount.UseRoundEnergy(Data.Energy());
            m_CardRectTransform.sizeDelta = defaultDeltaSize;

            OnDeselect?.Invoke();
        }
    }

    // destroy o objeto se estiver selecionado no inicio do proximo turno
    public void OnStartTurn()
    {
        if (selected)
        {
            Destroy(gameObject);
        }
    }

    // limpa as callbacks
    private void OnDestroy()
    {
        LanguageManager.Instance.OnChangeLanguage -= UpdateText;

        OnSelect = null;
        OnDeselect = null;
    }
}
