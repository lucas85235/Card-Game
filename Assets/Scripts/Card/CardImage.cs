using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardImage : MonoBehaviour
{
    [Header("Setup")]
    public Text title;
    public Text energy;

    [Header("Setup")]
    public Transform selectConteriner;
    public Transform selectedConteriner;
    public Energy energyCount;

    [Header("Debug")]
    [SerializeField] private bool selected = false;

    public Action OnSelect;
    public Action OnDeselect;

    [HideInInspector] public CardData data;
    private bool m_canSelect = true;

    void Start()
    {
        title.text = data.Title();
        energy.text = data.Energy().ToString();

        GameController.i.OnStartTurn.AddListener(() => OnStartTurn());
        GameController.i.OnStartTurn.AddListener(() => m_canSelect = true);
        GameController.i.OnEndTurn.AddListener(() => m_canSelect = false);
    }

    public void UseEffects(Robot robot, Robot target = null)
    {
        foreach (var effect in data.Effects())
        {
            effect.UseEffect(robot, target);
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
        int decreaseEnergy = energyCount.EnergyRoundAmount - data.Energy();

        if (energyCount.EnergyRoundAmount >= data.Energy() &&
            decreaseEnergy >= 0 && !selected)
        {
            selected = !selected;
            energyCount.UseRoundEnergy(-data.Energy());
            transform.SetParent(selectedConteriner);
            OnSelect?.Invoke();
        }
    }

    public void Deselect()
    {
        if (selected)
        {
            selected = !selected;
            energyCount.UseRoundEnergy(data.Energy());
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
