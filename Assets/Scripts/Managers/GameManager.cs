using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Setup")]
    public Transform selectedConteriner;
    public float timeBetweenPlays = 1f;

    [Header("Chracters")]
    public Robot player;
    public Robot cpu;

    [Header("Events")]
    public UnityEvent OnEndTurn;
    public UnityEvent OnStartTurn;

    // Use this to get Robots in order
    private List<Robot> sortRobots;

    private Energy m_energy;
    private List<CardData> m_roundCards;

    void Start()
    {
        m_energy = FindObjectOfType<Energy>();
        m_energy.OnEndRound += EndTurnHandle;

        OrderBySpeed();

        OnStartTurn?.Invoke();
    }

    private void OrderBySpeed()
    {
        sortRobots = new List<Robot>();

        if (player.data.Speed() > cpu.data.Speed())
        {
            sortRobots.Add(player);
            sortRobots.Add(cpu);
        }
        else
        {
            sortRobots.Add(cpu);
            sortRobots.Add(player);
        }
    }

    private void EndTurnHandle()
    {
        OnEndTurn?.Invoke();

        StartCoroutine("UseRoundCards");

        OnStartTurn?.Invoke();
    }

    private void UseRoundCards()
    {
        foreach (var robot in sortRobots)
        {
            var shild = 0;
            m_roundCards = new List<CardData>();
            
            Debug.Log(robot.name);

            for (int i = 0; i < selectedConteriner.childCount; i++)
            {
                var data = selectedConteriner.GetChild(i).GetComponent<CardImage>().data;
                m_roundCards.Add(data);
                shild += data.Defence();
            }

            foreach (var card in m_roundCards)
            {
                robot.life.TakeDamage(card.Attack());
            }
        }
    }

    private void OnDestroy()
    {
        m_energy.OnEndRound -= EndTurnHandle;
    }
}
