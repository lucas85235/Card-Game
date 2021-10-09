using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
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

    public static GameController i;

    private void Awake()
    {
        i = this;
    }

    void Start()
    {
        m_energy = FindObjectOfType<Energy>();
        m_energy.OnEndRound += EndTurnHandle;

        OrderBySpeed();

        OnStartTurn?.Invoke();
    }

    // Ordena a ordem de ataque dos robos de acordo com a velocidade
    private void OrderBySpeed()
    {
        sortRobots = new List<Robot>();

        if (player.data.speed > cpu.data.speed)
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

    // Logica usada apos clicar em end turn
    private void EndTurnHandle()
    {
        OnEndTurn?.Invoke();

        // quando começar o proximo round o CardImage selecionado e destruido
        // se for destruido antes de usado terá um problema

        if (selectedConteriner.childCount > 0)
            StartCoroutine(Plays());
    }

    // Determina a sequencia de jogadas e um tempo entre elas
    private IEnumerator Plays()
    {
        Debug.Log("F1");
        UseRoundCards(sortRobots[0], sortRobots[1]);

        if (sortRobots[1].life.isDeath)
            yield break;

        yield return new WaitForSeconds(timeBetweenPlays);

        Debug.Log("F2");
        UseRoundCards(sortRobots[1], sortRobots[0]);

        if (sortRobots[1].life.isDeath)
            yield break;

        yield return new WaitForSeconds(timeBetweenPlays);

        OnStartTurn?.Invoke();
    }

    // Pega as cartas do robo atual e aplica o dano ao proximo robo
    private void UseRoundCards(Robot currentRobot, Robot nextRobot)
    {
        m_roundCards = new List<CardData>();

        for (int i = 0; i < selectedConteriner.childCount; i++)
        {
            var data = selectedConteriner.GetChild(i).GetComponent<CardImage>().data;
            m_roundCards.Add(data);
        }

        foreach (var card in m_roundCards)
        {
            nextRobot.life.TakeDamage(card.attack);
        }
    }

    private void OnDestroy()
    {
        m_energy.OnEndRound -= EndTurnHandle;
        OnEndTurn.RemoveAllListeners();
        OnStartTurn.RemoveAllListeners();
    }
}
