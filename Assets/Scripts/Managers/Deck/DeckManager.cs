using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    private DeckHandle m_deck;
    private Energy m_energy;
    private Transform m_spaw;

    [Header("Setup")]
    public Transform selectConteriner;
    public Transform selectedConteriner;
    public CardImage cardTemplate;

    public DeckOf deckOf;

    private void Awake()
    {
        m_spaw = transform;

        if (deckOf == DeckOf.player)
            m_deck = GameObject.FindGameObjectWithTag("Player").GetComponent<DeckHandle>();
        else m_deck = GameObject.FindGameObjectWithTag("Cpu").GetComponent<DeckHandle>();

        if (deckOf == DeckOf.player)
            m_energy = GameObject.FindGameObjectWithTag("Player").GetComponent<Energy>();
        else m_energy = GameObject.FindGameObjectWithTag("Cpu").GetComponent<Energy>();

        m_deck.OnUpdateHands += UpdateDeck;

        if (deckOf == DeckOf.cpu)
        {
            Round.i.StartTurn.AddListener(() =>
               selectedConteriner.gameObject.SetActive(true)
            );
            Round.i.EndTurn.AddListener(() =>
               selectedConteriner.gameObject.SetActive(false)
            );

            selectedConteriner.gameObject.SetActive(false);
        }

        // Add Use Card Feedback Event
        Round.i.UseCard.AddListener( (card) => UseCardFeedback(card) );
    }

    // UseCard Event
    private void UseCardFeedback(CardImage card)
    {
        card.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    private void UpdateDeck(List<CardData> cards)
    {
        // destroy old cards
        if (m_spaw.childCount > 0)
        {
            for (int i = m_spaw.childCount - 1; i >= 0; i--)
            {
                Destroy(m_spaw.GetChild(i).gameObject);
            }
        }

        List<CardImage> spawCards = new List<CardImage>();

        // spaw new cards
        foreach (var card in cards)
        {
            CardImage cardImage = Instantiate(cardTemplate, Vector3.zero, Quaternion.identity, m_spaw);
            cardImage.energyCount = m_energy;
            cardImage.selectConteriner = selectConteriner;
            cardImage.selectedConteriner = selectedConteriner;
            cardImage.data = card;

            spawCards.Add(cardImage);
        }

        if (deckOf == DeckOf.cpu)
        {
            foreach (var spaw in spawCards)
            {
                spaw.Select();
            }
        }
    }

    private void OnDestroy()
    {
        m_deck.OnUpdateHands -= UpdateDeck;
    }
}

public enum DeckOf
{
    player, 
    cpu,
}
