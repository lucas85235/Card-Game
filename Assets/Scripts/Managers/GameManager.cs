using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Setup")]
    public Transform selectedConteriner;

    [Header("Chracters")]
    public Life player;
    public Life cpu;

    private Energy m_energy;
    private List<CardData> m_roundCards;

    void Start()
    {
        m_energy = FindObjectOfType<Energy>();
        m_energy.OnEndRound += EndTurnHandle;
    }

    private void EndTurnHandle()
    {
        UseRoundCards();
    }

    private void UseRoundCards()
    {
        var shild = 0;
        m_roundCards = new List<CardData>();

        for (int i = 0; i < selectedConteriner.childCount; i++)
        {
            var data = selectedConteriner.GetChild(i).GetComponent<CardImage>().data;
            m_roundCards.Add(data);
            shild += data.defense;
        }

        foreach (var card in m_roundCards)
        {
            cpu.TakeDamage(card.attack);
        }
    }

    private void OnDestroy()
    {
        m_energy.OnEndRound -= EndTurnHandle;
    }
}
