using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoundLoopMP : Round
{
    [Header("CHARACTERS")]
    public RobotMP playerOne;
    public RobotMP playerTwo;

    [Header("Setup")]
    [SerializeField] private bool useTimer = true;
    [SerializeField] private float timeToPlay = 10f;
    [SerializeField] private Slider timeSlider;
    [SerializeField] private Text timeText;

    [Header("SETTINGS")]
    [Tooltip("In millisecondsDelay")] public int delayBetweenTasks = 1000;
    [Tooltip("In millisecondsDelay")] public int delayBetweenUseCards = 800;
    [Tooltip("In millisecondsDelay")] public int delayBetweenStatusEffects = 600;

    private TimerMP timer = new TimerMP();

    public List<RobotMP> sortRobots = new List<RobotMP>();

    private static int MAX_CARD_PRIORITY = 4;

    private IEnumerator Start()
    {
        yield return new WaitUntil( () => GameControllerMP.Instance.isReady);

        timer.Timer = timeToPlay;
        timeSlider.gameObject.SetActive(useTimer);
        timeSlider.maxValue = timeToPlay;
        
        if (useTimer)
        {
            EndTurn.AddListener(() => timer.SetTimerProperties());
            timer.SetTimerProperties();
        }

        StartTurn.AddListener(() =>
           StartTurnPlaysHandle()
        );

        EndTurn.AddListener(() =>
           EndTurnInternalHandle()
        );

        playerOne = GameControllerMP.Instance.robots[0];
        playerTwo = GameControllerMP.Instance.robots[1];

        sortRobots = GameControllerMP.Instance.robots;

        SortBySpeed();
    }

    private void Update()
    {
        timer.CountTimer(() =>
        {
            timeSlider.gameObject.SetActive(false);
            StartTurn?.Invoke();
        });

        if (timer.StartTimer)
        {
            float totalTime = timeToPlay;
            timeSlider.value = totalTime - (float) timer.TimerIncrementValue;
            timeText.text = timer.TimerIncrementValue.ToString("#");
        }
    }

    private async void StartTurnPlaysHandle()
    {
        await Task.Delay(delayBetweenTasks);

        // Use All Cards
        await PlaysAllCards();
        await Task.Delay(delayBetweenTasks);

        if (sortRobots[0].life.isDead || 
            sortRobots[1].life.isDead)
            return;

        // End Turn
        EndTurn?.Invoke();
    }

    /// <summary>Sort robot attack order according to current speed</summary>
    private void SortBySpeed()
    {
        // if (playerOne.CurrentRobotStats[Stats.speed] > playerTwo.CurrentRobotStats[Stats.speed])
        // {
        //     sortRobots.Add(playerOne);
        //     sortRobots.Add(playerTwo);
        // }
        // else if (playerOne.CurrentRobotStats[Stats.speed] < playerTwo.CurrentRobotStats[Stats.speed])
        // {
        //     sortRobots.Add(playerTwo);
        //     sortRobots.Add(playerOne);
        // }

        // else // if equals use coin flip logic
        // {
        //     int rand = Random.Range(0, 2);

        //     // 0 == playerOne
        //     // 1 == playerTwo

        //     if (rand == 0)
        //     {
        //         sortRobots.Add(playerOne);
        //         sortRobots.Add(playerTwo);
        //     }
        //     else
        //     {
        //         sortRobots.Add(playerTwo);
        //         sortRobots.Add(playerOne);
        //     }
        // }
    }

    /// <summary>This call plays function using sortRobots with parans</summary>
    private async Task PlaysAllCards()
    {
        await GetAndPlaysAllRoundCards();
        await Task.Delay(delayBetweenTasks);

        return;
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

                if (card.ConnectedRobot.life.isDead)
                {
                    return true;
                }
            }
        }

        foreach (var robot in sortRobots)
        {
            // await robot.ActivateLateStatusEffects(delayBetweenStatusEffects);
        }

        return true;
    }

    /// <summary>Reset shild of all sortRobots list</summary>
    private void RemoveShield()
    {
        if (sortRobots == null) return;

        foreach (var robot in sortRobots)
        {
            // robot.life.RemoveShild();
        }
    }

    private void EndTurnInternalHandle()
    {
        RemoveShield();
    }
}
