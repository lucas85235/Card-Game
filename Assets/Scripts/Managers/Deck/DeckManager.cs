using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("Setup")]
    public Transform selectConteriner;
    public CardImage cardTemplate;

    public DeckOf deckOf;
    private Energy _energy;
    private DeckHandle _deck;
    private Robot _connectedRobot;

    private void Awake()
    {
        _deck = GetComponent<DeckHandle>();
        _energy = GetComponent<Energy>();
        _connectedRobot = GetComponent<Robot>();

        _deck.OnUpdateHands += UpdateDeck;
    }

    private void Start()
    {
        if (deckOf == DeckOf.cpu)
        {
            Round.i.StartTurn.AddListener(() =>
               selectConteriner.gameObject.SetActive(true)
            );
            Round.i.EndTurn.AddListener(() =>
               selectConteriner.gameObject.SetActive(false)
            );

            selectConteriner.gameObject.SetActive(false);
        }

        // Add Use Card Feedback Event
        Round.i.UseCard.AddListener(UseCardFeedback);
    }

    // UseCard Event
    private void UseCardFeedback(CardImage card)
    {
        card.gameObject.TryGetComponent(out RectTransform cardTransform);

        cardTransform.sizeDelta *= 1.2f;

        // StartCoroutine(MoveCard(cardTransform));
    }

    private IEnumerator MoveCard(RectTransform cardTransform)
    {
        yield return null;
        cardTransform.localPosition = new Vector3(cardTransform.localPosition.x, cardTransform.localPosition.y - (cardTransform.rect.height / 18f), cardTransform.localPosition.z);
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

        // spaw new cards
        foreach (var card in cards)
        {
            CardImage cardImage = Instantiate(cardTemplate, Vector3.zero, Quaternion.identity, selectConteriner);
            cardImage.energyCount = _energy;
            cardImage.selectConteriner = selectConteriner;
            cardImage.Data = card;
            cardImage.ConnectedRobot = _connectedRobot;

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

    private void OnDestroy()
    {
        Round.i.UseCard.RemoveListener(UseCardFeedback);
        _deck.OnUpdateHands -= UpdateDeck;
    }
}

public enum DeckOf
{
    player,
    cpu,
}
