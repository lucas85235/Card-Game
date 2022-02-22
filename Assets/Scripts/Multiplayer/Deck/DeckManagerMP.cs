using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Random = UnityEngine.Random;

public class DeckManagerMP : MonoBehaviour
{
    [Header("Temp")]
    [SerializeField] private RobotData m_Data;

    [Header("Setup")]
    public Transform selectConteriner;
    public Transform selectedConteriner;
    public CardImage cardTemplate;

    [Header("Lists")]
    [SerializeField] private List<CardData> deck;
    [SerializeField] private List<CardData> hands;
    [SerializeField] private List<CardData> discard;

    [Header("Hud Setup")]
    public TextMeshProUGUI deckText;
    public TextMeshProUGUI discardText;

    private Energy m_energy;

    public Action<List<CardData>> OnUpdateHands;

    private void Start()
    {
        m_energy = GetComponent<Energy>();

        SortDeck(true);

        // Call the turn every EndTurn
        // Round.i.EndTurn.AddListener(() => Turn());

        Turn();
    }

    private void SortDeck(bool applySkills)
    {
        deck = new List<CardData>();
        discard = new List<CardData>();

        // needed to reset handle after finishi deck
        hands = new List<CardData>();

        foreach (var card in m_Data.Cards()) // (var card in robot.CurrentCards)
        {
            deck.Add(card);

            if (!applySkills)
            {
                continue;
            }

            foreach (var skill in card.Skills())
            {
                if(skill != null)
                {
                    skill.ApplySkill(card);
                }
            }
        }

        // randomly order the deck
        deck.Sort((a, b) => 1 - 2 * Random.Range(0, 1));
    }

    private void Turn()
    {
        // add cards when you don't have enough
        if (deck.Count < 5)
        {
            SortDeck(false);
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

        deckText.text = LanguageManager.Instance.GetKeyValue("battle_deck") + ": " + deck.Count;
        discardText.text = LanguageManager.Instance.GetKeyValue("battle_discard") + ": " + discard.Count;

        Invoke("UpdateHands", 0.1f);
    }

    private void UpdateHands()
    {
        UpdateDeck(hands);
    }

    private void UpdateDeck(List<CardData> cards)
    {
        // destroy old cards
        if (selectConteriner.childCount > 0)
        {
            for (int i = selectConteriner.childCount - 1; i >= 0; i--)
            {
                Destroy(selectConteriner.GetChild(i).gameObject);
            }
        }

        List<CardImage> spawCards = new List<CardImage>();

        // Spaw new cards
        foreach (var card in cards)
        {
            CardImage cardImage = Instantiate(cardTemplate, Vector3.zero, Quaternion.identity, selectConteriner);
            cardImage.energyCount = m_energy;
            cardImage.selectConteriner = selectConteriner;
            cardImage.selectedConteriner = selectedConteriner;
            cardImage.Data = card;
            
            // cardImage.ConnectedRobot = m_ConnectedRobot;

            spawCards.Add(cardImage);
        }
    }

    // Randomly select current hand order
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
}
