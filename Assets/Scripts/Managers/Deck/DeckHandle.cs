using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class DeckHandle : MonoBehaviour
{
    // The deck it has 20 cars
    // for each turn its buy 5
    // at the end of each turn, the cards that are not used and those that are discarded
    // as soon as the deck has less than 5 cards they will be reshuffled and drawn
    // you will buy the ones you have and the ones that are shuffled to complete 5

    [Header("Lists")]
    [SerializeField] private List<CardData> deck;
    [SerializeField] private List<CardData> hands;
    [SerializeField] private List<CardData> discard;

    [Header("Hud Setup")]
    public Text deckText;
    public Text discardText;

    public Action<List<CardData>> OnUpdateHands;

    private Robot robot;

    private void Start()
    {
        robot = GetComponent<Robot>();

        SortDeck();

        // Call the turn every EndTurn
        Round.i.EndTurn.AddListener(() => Turn());

        Turn();
    }

    private void SortDeck()
    {
        deck = new List<CardData>();
        discard = new List<CardData>();

        // needed to reset handle after finishi deck
        hands = new List<CardData>();

        foreach (var card in robot.data.Cards())
        {
            deck.Add(card);
        }

        // randomly order the deck
        deck.Sort((a, b) => 1 - 2 * Random.Range(0, 1));
    }

    private void Turn()
    {
        // add cards when you don't have enough
        if (deck.Count < 5)
        {
            SortDeck();
        }

        // add the cards from the hand to the discard
        foreach (var card in hands)
        {
            discard.Add(card);
        }

        hands = new List<CardData>();
        var deckSelect = GetRandomHandsList();

        // add the hand
        foreach (var s in deckSelect)
        {
            hands.Add(deck[s]);
        }

        // strip from the deck
        foreach (var s in deckSelect)
        {
            deck.RemoveAt(s);
        }

        DeckText("Deck: " + deck.Count);
        DiscarText("Discard: " + discard.Count);

        Invoke("UpdateHands", 0.1f);
    }

    public void UpdateHands()
    {
        // calls the event that updates the cards in the hand
        OnUpdateHands(hands);          
    }

    // randomly select current hand order
    private List<int> GetRandomHandsList()
    {
        var deckSelect = new List<int>();

        while (deckSelect.Count < 5)
        {
            var r = Random.Range(0, deck.Count);

            if (!deckSelect.Contains(r))
                deckSelect.Add(r);
        }

        deckSelect.Sort();
        deckSelect.Reverse();

        return deckSelect;
    }

    private void DeckText(string text) 
    {
        if (deckText != null)
        {
            deckText.text = text;
        }
    }

    private void DiscarText(string text) 
    {
        if (discardText != null)
        {
            discardText.text = text;
        }
    }

    private void OnDestroy()
    {
        OnUpdateHands = null;
    }
}
