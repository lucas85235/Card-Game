using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoundLoopMP : Round
{
    [Header("SETTINGS")]

    [Tooltip("In millisecondsDelay")]
    public int delayBetweenTasks = 1000;

    [Tooltip("In millisecondsDelay")]
    public int delayBetweenUseCards = 800;

    [Tooltip("In millisecondsDelay")]
    public int delayBetweenStatusEffects = 600;

    [Header("Setup")]
    [SerializeField] protected bool useTimer = true;
    [SerializeField] protected float timeToPlay = 10f;
    [SerializeField] protected Slider timeSlider;

    private TimerMP timer = new TimerMP();
    private Transform selectedConterinerPlayerOne;
    private Transform selectedConterinerPlayerTwo;
    private List<Robot> sortRobots;
    private PhotonView view;
    
    private static int MAX_CARD_PRIORITY = 4;

    protected virtual IEnumerator Start()
    {
        view = GetComponent<PhotonView>();

        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        // yield return new WaitUntil(() => PhotonNetwork.PlayerList.Length > 0);
        yield return new WaitUntil(() => PhotonNetwork.PlayerList.Length == PhotonNetwork.CurrentRoom.MaxPlayers);
        yield return new WaitForSeconds(0.25f);

        Debug.Log("Sala cheia");

        if (PhotonNetwork.IsMasterClient)
        {
            view.RPC("SetRobots", RpcTarget.All);
            Round.i.EndTurn.AddListener(() => timer.SetTimerProperties());
            timer.SetTimerProperties();
        }

        yield return new WaitForSeconds(0.25f);

        timer.Timer = timeToPlay;
        timeSlider.gameObject.SetActive(useTimer);
        timeSlider.maxValue = timeToPlay;

        sortRobots = new List<Robot>();
        SortBySpeed();

        selectedConterinerPlayerOne = playerOne.selectedCardsConteriner;
        selectedConterinerPlayerTwo = playerTwo.selectedCardsConteriner;

        StartTurn.AddListener(() =>
           StartTurnPlaysHandle()
        );

        EndTurn.AddListener(() =>
           EndTurnInternalHandle()
        );

        if (useTimer && !PhotonNetwork.IsMasterClient)
        {
            Round.i.EndTurn.AddListener(() => timer.SetTimerProperties());
            timer.SetTimerProperties();
        }

        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var robots in sortRobots)
            {
                robots.GetComponent<LifeMP>().UpdateLifeSlider();
            }

            view.RPC("SetReady", RpcTarget.All);
        }
    }

    private void Update()
    {
        timer.CountTimer(() =>
        {
            timeSlider.gameObject.SetActive(false);
            StartTurn?.Invoke();
            Debug.Log("Invoke StartTurn");
        });

        if (timer.StartTimer)
        {
            float totalTime = timeToPlay;
            timeSlider.value = totalTime - (float) timer.TimerIncrementValue;
        }
    }

    [PunRPC]
    private void SetReady()
    {
        isReady = true;
    }

    [PunRPC]
    private void SetRobots()
    {
        var robots = new List<Robot>(FindObjectsOfType<Robot>());

        playerOne = robots[0];
        playerTwo = robots[1];
    }

    private async void StartTurnPlaysHandle()
    {
        Debug.Log("Start Event Call");

        timeSlider.gameObject.SetActive(true);

        await Task.Delay(delayBetweenTasks);

        // Use All Cards
        await GetAndPlaysAllRoundCards();
        await Task.Delay(delayBetweenTasks);

        if (sortRobots[0].life.isDead ||
            sortRobots[1].life.isDead)
            {
                Debug.Log("DEAD RETURN");
                return;
            }

        Debug.Log("END");

        // End Turn
        EndTurn?.Invoke();
    }

    /// <summary>Sort robot attack order according to current speed</summary>
    private void SortBySpeed()
    {
        if (playerOne.CurrentRobotStats[Stats.speed] > playerTwo.CurrentRobotStats[Stats.speed])
        {
            sortRobots.Add(playerOne);
            sortRobots.Add(playerTwo);
        }
        else if (playerOne.CurrentRobotStats[Stats.speed] < playerTwo.CurrentRobotStats[Stats.speed])
        {
            sortRobots.Add(playerTwo);
            sortRobots.Add(playerOne);
        }

        else // if equals use coin flip logic
        {
            int rand = Random.Range(0, 2);

            // 0 == playerOne
            // 1 == playerTwo

            sortRobots.Add(rand == 0 ? playerOne : playerTwo);
            sortRobots.Add(rand == 0 ? playerTwo : playerOne);
        }
    }

    /// <summary>Get all cards of the current robot and apply effects to the target</summary>
    public async Task<bool> GetAndPlaysAllRoundCards()
    {
        var m_roundCards = new Dictionary<int, List<CardImage>>();

        for (int i = 0; i < sortRobots.Count; i++)
        {
            for (int j = 0; j < sortRobots[i].selectedCardsConteriner.childCount; j++)
            {
                sortRobots[i].selectedCardsConteriner.GetChild(j).TryGetComponent(out CardImage cardImage);

                if (!m_roundCards.ContainsKey(cardImage.Data.Priority))
                {
                    m_roundCards[cardImage.Data.Priority] = new List<CardImage>();
                }
                m_roundCards[cardImage.Data.Priority].Add(cardImage);
            }
        }

        for (int i = 0; i <= MAX_CARD_PRIORITY; i++)
        {
            if (!m_roundCards.ContainsKey(i))
            {
                continue;
            }

            foreach (var card in m_roundCards[i])
            {
                UseCard.Invoke(card);

                // Robot Attack Feedback Events
                RobotAttack.Invoke(card.ConnectedRobot, GameController.i.GetTheOtherRobot(card.ConnectedRobot));

                await Task.Delay(delayBetweenUseCards);

                card.UseEffect();
                card.gameObject.SetActive(false);

                if (card.Data.SingleUse)
                {
                    card.ConnectedRobot.RemoveCard(card.Data);
                }

                if (sortRobots[0].life.isDead || sortRobots[1].life.isDead)
                {
                    return true;
                }
            }
        }

        foreach (var robot in sortRobots)
        {
            await robot.ActivateLateStatusEffects(delayBetweenStatusEffects);
        }

        return true;
    }

    /// <summary>Reset shild of all sortRobots list</summary>
    private void RemoveShield()
    {
        if (sortRobots == null) return;

        foreach (var robot in sortRobots)
        {
            robot.life.RemoveShild();
        }
    }

    private void EndTurnInternalHandle()
    {
        RemoveShield();
        Debug.Log("End Event Call");
    }
}
