using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [Header("Setup")]
    public Transform selectedConterinerPlayer;
    public Transform selectedConterinerCpu;
    public float timeBetweenPlays = 1f;

    [Header("Chracters")]
    public Robot player;
    public Robot cpu;

    [Header("Events")]
    public UnityEvent OnEndTurn;
    public UnityEvent OnStartTurn;

    // Use this to get Robots in order
    private List<Robot> sortRobots;

    private bool inActionPlays = false;

    public static GameController i;

    private void Awake()
    {
        i = this;
    }

    void Start()
    {
        AudioManager.Instance.Play(AudiosList.gameplayMusic, isMusic: true);
        AudioManager.Instance.ChangeMusicVolumeWithLerp(1, 3f, startVolume: 0);

        player.energy.OnEndRound += EndTurnHandle;

        OrderBySpeed();

        cpu.selectedConteriner = selectedConterinerCpu;
        player.selectedConteriner = selectedConterinerPlayer;

        OnStartTurn?.Invoke();

        OnStartTurn.AddListener(() =>
        {
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

        if (player.data.Speed() == cpu.data.Speed())
        {
            int r = Random.Range(0, 2);

            if (r == 0)
                player.SpeedBuff(1);
            else cpu.SpeedBuff(1);

            Debug.Log("Sort Speed " + r);
        }

        if (player.Speed() > cpu.Speed())
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

        OrderBySpeed();

        // quando começar o proximo round o CardImage selecionado e destruido
        // se for destruido antes de usado terá um problema

        if (selectedConterinerPlayer.childCount > 0 || selectedConterinerCpu.childCount > 0)
            StartCoroutine(Plays());
        else OnStartTurn?.Invoke();
    }

    // Determina a sequencia de jogadas e um tempo entre elas
    private IEnumerator Plays()
    {
        inActionPlays = false;

        for (int i = 0; i < selectedConterinerPlayer.childCount; i++)
        {
            var card = selectedConterinerPlayer.GetChild(i).GetComponent<CardImage>();
            // player.life.AddShild(data.Defence());
            card.UseEffects(player, cpu, true);
        }

        for (int i = 0; i < selectedConterinerCpu.childCount; i++)
        {
            var card = selectedConterinerCpu.GetChild(i).GetComponent<CardImage>();
            // cpu.life.AddShild(data.Defence());
            card.UseEffects(cpu, player, true);
        }

        yield return new WaitForSeconds(timeBetweenPlays / 2);

        // Debug.Log("F1");
        inActionPlays = true;
        StartCoroutine(
            sortRobots[0].UseRoundCards(sortRobots[1], (value) =>
            {
                inActionPlays = value;
            })
        );

        yield return new WaitUntil(() => inActionPlays == false);
        yield return new WaitForSeconds(timeBetweenPlays);

        if (sortRobots[1].life.isDeath)
            yield break;

        // Debug.Log("F2");
        inActionPlays = true;
        StartCoroutine(
            sortRobots[1].UseRoundCards(sortRobots[0], (value) =>
            {
                inActionPlays = value;
            })
        );

        yield return new WaitUntil(() => inActionPlays == false);
        yield return new WaitForSeconds(timeBetweenPlays);

        if (sortRobots[1].life.isDeath)
            yield break;

        OnStartTurn?.Invoke();
    }

    private void OnDestroy()
    {
        player.energy.OnEndRound -= EndTurnHandle;

        OnEndTurn.RemoveAllListeners();
        OnStartTurn.RemoveAllListeners();

        OnEndTurn = null;
        OnStartTurn = null;
    }
}
