using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardImage : MonoBehaviour
{
    [Header("Data")]
    public CardData data;

    [Header("Setup")]
    public Text title;
    public Text energy;
    public Text attack;
    public Text defense;

    [Header("Setup")]
    public Transform selectConteriner;
    public Transform selectedConteriner;

    [Header("Debug")]
    [SerializeField] private bool selected = false;

    public Action OnSelect;
    public Action OnDeselect;

    private Energy m_energy;

    void Start()
    {
        m_energy = FindObjectOfType<Energy>();

        title.text = data.Title();
        energy.text = "Energy: " + data.Energy();
        attack.text = "Attack: " + data.Attack();
        defense.text = "Defense: " + data.Defence();

        m_energy.OnEndRound += OnEndTurn;
    }

    public void OnPointerEnter()
    {
        transform.localScale = new Vector3(1.2f, 1.2f);
    }

    public void OnPointerExit()
    {
        transform.localScale = new Vector3(1f, 1f);
    }

    public void OnClick()
    {
        if (!selected)
        {
            Select();
        }
        else Deselect();
    }

    public void Select()
    {
        int decreaseEnergy = m_energy.EnergyRoundAmount - data.Energy();

        if (m_energy.EnergyRoundAmount >= data.Energy() &&
            decreaseEnergy >= 0 && !selected)
        {
            selected = !selected;
            m_energy.UseRoundEnergy(-data.Energy());
            transform.SetParent(selectedConteriner);
            OnSelect?.Invoke();
        }
    }

    public void Deselect()
    {
        if (selected)
        {
            selected = !selected;
            m_energy.UseRoundEnergy(data.Energy());
            transform.SetParent(selectConteriner);
            OnDeselect?.Invoke();
        }
    }

    public void OnEndTurn()
    {
        if (selected)
        {
            Destroy(this.gameObject, 0.1f);
        }
    }

    private void OnDestroy()
    {
        OnSelect = null;
        OnDeselect = null;
        m_energy.OnEndRound -= OnEndTurn;

    }
}
