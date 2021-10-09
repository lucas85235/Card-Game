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
    private List<CardImage> m_roundCards;
    private bool inActionPlays = false;

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

        OnStartTurn.AddListener(() => {
            foreach (var robot in sortRobots)
            {
                robot.life.RemoveShild();
            }
        });
    }

    // Ordena a ordem de ataque dos robos de acordo com a velocidade
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

    // Logica usada apos clicar em end turn
    private void EndTurnHandle()
    {
        OnEndTurn?.Invoke();

        // quando começar o proximo round o CardImage selecionado e destruido
        // se for destruido antes de usado terá um problema

        if (selectedConteriner.childCount > 0)
            StartCoroutine(Plays());
        else OnStartTurn?.Invoke();
    }

    // Determina a sequencia de jogadas e um tempo entre elas
    private IEnumerator Plays()
    {
        inActionPlays = false;

        foreach (var robots in sortRobots)
        {
            for (int i = 0; i < selectedConteriner.childCount; i++)
            {
                var data = selectedConteriner.GetChild(i).GetComponent<CardImage>().data;
                robots.life.AddShild(data.Defence());
            }        
        }

        yield return new WaitForSeconds(timeBetweenPlays);

        Debug.Log("F1");
        inActionPlays = true;
        StartCoroutine(
            UseRoundCards(sortRobots[0], sortRobots[1])
        );

        yield return new WaitUntil(() => inActionPlays == false);
        yield return new WaitForSeconds(timeBetweenPlays);

        if (sortRobots[1].life.isDeath)
            yield break;

        // Debug.Log("F2");
        inActionPlays = true;
        StartCoroutine(
            UseRoundCards(sortRobots[1], sortRobots[0])
        );

        yield return new WaitUntil(() => inActionPlays == false);
        yield return new WaitForSeconds(timeBetweenPlays);

        if (sortRobots[1].life.isDeath)
            yield break;

        OnStartTurn?.Invoke();
    }

    // Pega as cartas do robo atual e aplica o dano ao proximo robo
    private IEnumerator UseRoundCards(Robot currentRobot, Robot nextRobot)
    {
        m_roundCards = new List<CardImage>();

        for (int i = 0; i < selectedConteriner.childCount; i++)
        {
            var data = selectedConteriner.GetChild(i).GetComponent<CardImage>();
            m_roundCards.Add(data);
        }

        foreach (var card in m_roundCards)
        {
            card.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            nextRobot.life.TakeDamage(card.data.Attack());
            yield return new WaitForSeconds(1f);
            card.gameObject.SetActive(false);
        }

        inActionPlays = false;
    }

    private void OnDestroy()
    {
        m_energy.OnEndRound -= EndTurnHandle;

        OnEndTurn.RemoveAllListeners();
        OnStartTurn.RemoveAllListeners();

        OnEndTurn = null;
        OnStartTurn = null;
    }
}
