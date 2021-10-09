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
    private bool canSelect = true;

    void Start()
    {
        m_energy = FindObjectOfType<Energy>();

        title.text = data.title;
        energy.text = "Energy: " + data.energy;
        attack.text = "Attack: " + data.attack;
        defense.text = "Defense: " + data.defense;

        GameController.i.OnStartTurn.AddListener(() => OnStartTurn());
        GameController.i.OnStartTurn.AddListener(() => canSelect = true);
        GameController.i.OnEndTurn.AddListener(() => canSelect = false);
    }

    public void OnPointerEnter()
    {
        if (!canSelect) return;
        transform.localScale = new Vector3(1.2f, 1.2f);
    }

    public void OnPointerExit()
    {
        if (!canSelect) return;
        transform.localScale = new Vector3(1f, 1f);
    }

    public void OnClick()
    {
        // Verifica se está no turno
        if (!canSelect) return;

        if (!selected)
        {
            Select();
        }
        else Deselect();
    }

    public void Select()
    {
        int decreaseEnergy = m_energy.EnergyRoundAmount - data.energy;

        if (m_energy.EnergyRoundAmount >= data.energy &&
            decreaseEnergy >= 0 && !selected)
        {
            selected = !selected;
            m_energy.UseRoundEnergy(-data.energy);
            transform.SetParent(selectedConteriner);
            OnSelect?.Invoke();
        }
    }

    public void Deselect()
    {
        if (selected)
        {
            selected = !selected;
            m_energy.UseRoundEnergy(data.energy);
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
