using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckManager : MonoBehaviour
{
    [Header("Setup")]
    public Transform selectConteriner;
    public Transform selectedConteriner;
    public CardImage cardTemplate;
    public DeckOf deckOf;

    [Header("Hud Setup")]
    public TextMeshProUGUI deckText;
    public TextMeshProUGUI discardText;

    [Header("Lists")]
    protected List<CardData> cardsInDeck;
    protected List<CardData> cardsInHand;
    protected List<CardData> cardsInDiscard;

    protected Energy m_energy;
    protected Robot m_ConnectedRobot;

    protected virtual void Awake()
    {
        m_energy = GetComponent<Energy>();
        m_ConnectedRobot = GetComponent<Robot>();
    }

    protected virtual void Start()
    {
        SortDeck(true);

        // Call the turn every EndTurn
        Round.i.EndTurn.AddListener(() => Turn());

        Turn();

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

    protected virtual void SortDeck(bool applySkills)
    {
        cardsInDeck = new List<CardData>();
        cardsInDiscard = new List<CardData>();

        // needed to reset handle after finishi deck
        cardsInHand = new List<CardData>();

        foreach (var card in m_ConnectedRobot.CurrentCards)
        {
            cardsInDeck.Add(card);

            if (!applySkills) 
            {
                continue;
            }
            
            foreach (var skill in card.Skills())
            {
                if(skill != null)
                    skill.ApplySkill(card);
            }
        }

        // randomly order the deck
        cardsInDeck.Sort((a, b) => 1 - 2 * Random.Range(0, 1));
    }

    protected virtual void Turn()
    {
        // add cards when you don't have enough
        if (cardsInDeck.Count < 5)
        {
            SortDeck(false);
        }

        // add the cards from the hand to the discard
        foreach (var card in cardsInHand)
        {
            cardsInDiscard.Add(card);
        }

        cardsInHand = new List<CardData>();
        var deckSelect = GetRandomHandsList();

        // add the hand
        foreach (var s in deckSelect)
        {
            cardsInHand.Add(cardsInDeck[s]);
        }

        // strip from the deck
        foreach (var s in deckSelect)
        {
            cardsInDeck.RemoveAt(s);
        }

        deckText.text = LanguageManager.Instance.GetKeyValue("battle_deck") + ": " + cardsInDeck.Count;
        discardText.text = LanguageManager.Instance.GetKeyValue("battle_discard") + ": " + cardsInDiscard.Count;

        Invoke("UpdateDeck", 0.1f); // UPDATE THISSSSSS
    }

    // randomly select current hand order
    protected virtual List<int> GetRandomHandsList()
    {
        var deckSelect = new List<int>();

        while (deckSelect.Count < 5)
        {
            var r = Random.Range(0, cardsInDeck.Count);

            if (!deckSelect.Contains(r))
                deckSelect.Add(r);
        }

        deckSelect.Sort();
        deckSelect.Reverse();

        return deckSelect;
    }

    // UseCard Event
    protected virtual void UseCardFeedback(CardImage card)
    {
        card.gameObject.TryGetComponent(out RectTransform cardTransform);

        cardTransform.sizeDelta *= 1.2f;
        StartCoroutine(MoveCard(cardTransform));
    }

    protected virtual IEnumerator MoveCard(RectTransform cardTransform)
    {
        yield return null;
        cardTransform.localPosition = new Vector3(cardTransform.localPosition.x, cardTransform.localPosition.y - (cardTransform.rect.height / 18f), cardTransform.localPosition.z);
    }

    protected virtual void UpdateDeck()
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

        // spaw new cards
        foreach (var card in cardsInHand)
        {
            CardImage cardImage = Instantiate(cardTemplate, Vector3.zero, Quaternion.identity, selectConteriner);
            cardImage.energyCount = m_energy;
            cardImage.selectConteriner = selectConteriner;
            cardImage.selectedConteriner = selectedConteriner;
            cardImage.Data = card;
            cardImage.ConnectedRobot = m_ConnectedRobot;

            spawCards.Add(cardImage);
        }

        if (deckOf == DeckOf.cpu)
        {
            foreach (var spaw in spawCards)
            {
                spaw.SetCanSelect(false);
                spaw.Select();
            }
        }
    }
}

public enum DeckOf
{
    player, 
    cpu,
}
