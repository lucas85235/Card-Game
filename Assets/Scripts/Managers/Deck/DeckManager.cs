using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DeckManager : MonoBehaviour
{
    private DeckHandle m_deck;
    private Energy m_energy;
    private Transform m_spaw;

    [Header("Setup")]
    public Transform selectConteriner;
    public Transform selectedConteriner;
    public CardImage cardTemplate;
    public Transform player;

    [Header("IA")]
    public bool isAI;

    private Robot m_ConnectedRobot;
    private PhotonView m_view;

    private void Awake()
    {
        m_spaw = transform;
        m_view = player.GetComponent<PhotonView>();

        if (m_view != null && !m_view.IsMine)
        {
            var rect = GetComponent<RectTransform>();
            var rectPos = rect.localPosition;
            rectPos.y -= 500;
            rect.localPosition = rectPos;
        }
    }

    private IEnumerator Start()
    {
        if (GameController.i.isMultiplayer)
        {
            yield return new WaitUntil(() => BasicConection.Instance.IsReady());
        }

        m_deck = player.GetComponent<DeckHandle>();
        m_energy = player.GetComponent<Energy>();
        m_ConnectedRobot = player.GetComponent<Robot>();

        m_deck.OnUpdateHands += UpdateDeck;

        if (isAI)
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
        Round.i.UseCard.AddListener((card) => UseCardFeedback(card));
    }

    // UseCard Event
    private void UseCardFeedback(CardImage card)
    {
        card.gameObject.TryGetComponent(out RectTransform cardTransform);

        cardTransform.sizeDelta *= 1.2f;
        StartCoroutine(MoveCard(cardTransform));
    }

    private IEnumerator MoveCard(RectTransform cardTransform)
    {
        yield return null;
        cardTransform.localPosition = new Vector3(cardTransform.localPosition.x, cardTransform.localPosition.y - (cardTransform.rect.height / 18f), cardTransform.localPosition.z);
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
            cardImage.Data = card;
            cardImage.ConnectedRobot = m_ConnectedRobot;

            spawCards.Add(cardImage);
        }

        if (isAI)
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
        m_deck.OnUpdateHands -= UpdateDeck;
    }
}

public enum DeckOf
{
    player,
    cpu,
}
