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
    public Image image;

    [Header("Setup")]
    public Transform selectConteriner;
    public Transform selectedConteriner;
    public Energy energyCount;

    [Header("Debug")]
    [SerializeField] private bool selected = false;

    public Action OnSelect;
    public Action OnDeselect;

    public CardData Data { get; set; }
    public Robot ConnectedRobot { get; set; }

    private bool m_canSelect = true;
    private bool m_canInteract = true;

    private RectTransform m_CardRectTransform;

    public void SetCanSelect(bool state) => m_canSelect = state;

    private void Awake()
    {
        TryGetComponent(out m_CardRectTransform);
    }

    void Start()
    {
        image.sprite = Data.Sprite();
        energy.text = Data.Energy().ToString();

        LanguageManager.Instance.OnChangeLanguage += UpdateText;
        UpdateText();

        Round.i.EndTurn.AddListener(() => OnStartTurn());
        Round.i.StartTurn.AddListener(() => m_canSelect = false);
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
            effect.UseEffect(ConnectedRobot, GameControllerMP.Instance.GetTheOtherRobot(ConnectedRobot), Data);
        }
    }

    public void OnPointerEnter()
    {
        if (!m_canSelect || !m_canInteract)
        {
            return;
        }

        var scaleOrientation = !selected ? 1 : -1;

        m_CardRectTransform.sizeDelta *= 2;
        StartCoroutine(MoveCard(scaleOrientation));
    }

    private IEnumerator MoveCard(float value)
    {
        yield return null;
        m_CardRectTransform.position = new Vector3(m_CardRectTransform.position.x, m_CardRectTransform.position.y + (m_CardRectTransform.rect.height / 9f) * value, m_CardRectTransform.position.z);
    }

    public void OnPointerExit()
    {
        if (!m_canSelect || !m_canInteract)
        {
            return;
        }

        m_CardRectTransform.sizeDelta *= .5f;
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

            energyCount.UseRoundEnergy(-Data.Energy());

            m_CardRectTransform.SetParent(selectedConteriner);

            OnSelect?.Invoke();
        }
    }

    public void Deselect()
    {
        if (selected)
        {
            AudioManager.Instance.Play(AudiosList.cardPush);

            selected = false;

            energyCount.UseRoundEnergy(Data.Energy());

            m_CardRectTransform.SetParent(selectConteriner);

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
