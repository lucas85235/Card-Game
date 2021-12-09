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

    void Start()
    {
        image.sprite = Data.Sprite();
        title.text = Data.Title();
        energy.text = Data.Energy().ToString();
        description.text = Data.Description();

        Round.i.EndTurn.AddListener(() => OnStartTurn());
        Round.i.StartTurn.AddListener(() => m_canSelect = false);
    }

    public void UseEffect()
    {
        foreach (var effect in Data.Effects())
        {
            effect.UseEffect(ConnectedRobot, GameController.i.GetTheOtherRobot(ConnectedRobot), Data);
        }
    }

    public void OnPointerEnter()
    {
        if (!m_canSelect) return;
        transform.localScale = new Vector3(1.2f, 1.2f);
    }

    public void OnPointerExit()
    {
        if (!m_canSelect) return;
        transform.localScale = new Vector3(1f, 1f);
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

            selected = !selected;
            energyCount.UseRoundEnergy(-Data.Energy());
            transform.SetParent(selectedConteriner);
            OnSelect?.Invoke();
        }
    }

    public void Deselect()
    {
        if (selected)
        {
            AudioManager.Instance.Play(AudiosList.cardPush);

            selected = !selected;
            energyCount.UseRoundEnergy(Data.Energy());
            transform.SetParent(selectConteriner);
            OnDeselect?.Invoke();
        }
    }

    // destroy o objeto se estiver selecionado no inicio do proximo turno
    public void OnStartTurn()
    {
        if (selected)
        {
            Destroy(this.gameObject);
        }
    }

    // limpa as callbacks
    private void OnDestroy()
    {
        OnSelect = null;
        OnDeselect = null;
    }
}
