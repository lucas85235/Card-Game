using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RoundLoop : Round
{
    [Header("CHARACTERS")]
    public Robot playerOne;
    public Robot playerTwo;

    [Header("SETTINGS")]

    [Tooltip("In millisecondsDelay")]
    public int delayBetweenTasks = 1000;

    [Tooltip("In millisecondsDelay")]
    public int delayBetweenUseCards = 800;

    [Header("CARDS")]
    public Transform selectedConterinerPlayerOne;
    public Transform selectedConterinerPlayerTwo;

    private List<Robot> sortRobots;

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Start()
    {
        selectedConterinerPlayerOne = playerOne.selectedCardsConteriner;
        selectedConterinerPlayerTwo = playerTwo.selectedCardsConteriner;

        StartTurn.AddListener(() =>
           StartTurnPlaysHandle()
        );

        EndTurn.AddListener(() =>
           EndTurnInternalHandle()
        );
    }

    private async void StartTurnPlaysHandle()
    {
        Debug.Log("StartTurnPlaysHandle");

        // Sort play Order 
        SortBySpeed();

        // Can be Implement Quick Cards Here

        // Apply Shild
        UseShildCards();
        await Task.Delay(delayBetweenTasks);

        // Use All Cards
        await PlaysAllCards();
        await Task.Delay(delayBetweenTasks);

        if (sortRobots[0].life.isDeath || 
            sortRobots[1].life.isDeath)
            return;

        // End Turn
        EndTurn?.Invoke();
    }

    /// <summary>Sort robot attack order according to current speed</summary>
    private void SortBySpeed()
    {
        sortRobots = new List<Robot>();

        if (playerOne.Speed() > playerTwo.Speed())
        {
            sortRobots.Add(playerOne);
            sortRobots.Add(playerTwo);
        }
        else if (playerOne.Speed() < playerTwo.Speed())
        {
            sortRobots.Add(playerTwo);
            sortRobots.Add(playerOne);
        }

        else // if equals use coin flip logic
        {
            int rand = Random.Range(0, 2);

            // 0 == playerOne
            // 1 == playerTwo

            if (rand == 0) playerOne.SpeedBuff(1);
            else playerTwo.SpeedBuff(1);

            Debug.Log("Sort Speed " + (rand == 0 ? "PlayerOne" : "PlayerTwo"));
        }
    }

    /// <summary>Get cards in container and apply shild if has</summary>
    private void UseShildCards()
    {
        try
        {
            if (sortRobots[0].selectedCardsConteriner.childCount < 0) return;

            for (int i = 0; i < sortRobots[0].selectedCardsConteriner.childCount; i++)
            {
                var temp = sortRobots[0].selectedCardsConteriner.GetChild(i);
                var current = temp.GetComponent<CardImage>();
                current.UseShildEffect(sortRobots[0], sortRobots[1]);
            }            
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            throw;
        }

        try
        {
            if (sortRobots[1].selectedCardsConteriner.childCount < 0) return;

            for (int i = 0; i < sortRobots[1].selectedCardsConteriner.childCount; i++)
            {
                var temp = sortRobots[1].selectedCardsConteriner.GetChild(i);
                var current = temp.GetComponent<CardImage>();
                current.UseShildEffect(sortRobots[1], sortRobots[0]);
            }           
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            throw;
        }
    }

    /// <summary>Create this method implemation later</summary>
    private List<CardImage> UseQuickCards(List<CardImage> cards, bool reverse = false)
    {
        List<Robot> robots = sortRobots;
        if (reverse) robots.Reverse();

        // if this card is quick use and remover from the list

        foreach (var card in cards)
        {
            // UseCard(card, robots);
        }

        return cards;
    }

    /// <summary>This call plays function using sortRobots with parans</summary>
    private async Task PlaysAllCards()
    {
        await GetAndPlaysAllRoundCards(sortRobots[0], sortRobots[1]);
        await Task.Delay(delayBetweenTasks);

        if (sortRobots[1].life.isDeath)
            return;

        await GetAndPlaysAllRoundCards(sortRobots[1], sortRobots[0]);
        await Task.Delay(delayBetweenTasks);

        if (sortRobots[1].life.isDeath)
            return;
    }

    /// <summary>Get all cards of the current robot and apply effects to the target</summary>
    public async Task<bool> GetAndPlaysAllRoundCards(Robot robot, Robot target)
    {
        var m_roundCards = new List<CardImage>();

        for (int i = 0; i < robot.selectedCardsConteriner.childCount; i++)
        {
            var data = robot.selectedCardsConteriner.GetChild(i).
                GetComponent<CardImage>();
            m_roundCards.Add(data);
        }

        foreach (var card in m_roundCards)
        {
            // Use Card FeedBack Passing Card In Use
            UseCard.Invoke(card);

            // Robot Attack Feedback Events
            RobotAttack.Invoke(robot, target);

            await Task.Delay(delayBetweenUseCards);
            
            card.UseEffect(robot, target);
            card.gameObject.SetActive(false);

            if (target.life.isDeath)
                return true;
        }

        return true;
    }

    /// <summary>Reset shild of all sortRobots list</summary>
    private void RemoveShild()
    {
        if (sortRobots == null) return;

        foreach (var robot in sortRobots)
        {
            robot.life.RemoveShild();
        }
    }

    private void EndTurnInternalHandle()
    {
        RemoveShild();
    }
}
