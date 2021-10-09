using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsInHands : MonoBehaviour
{
    private DeckHandle m_deck;
    private Transform m_spaw;

    [Header("Setup")]
    public Transform selectConteriner;
    public Transform selectedConteriner;

    private void Awake()
    {
        m_spaw = transform;
        m_deck = FindObjectOfType<DeckHandle>();
        m_deck.OnUpdateHands += UpdateDeck;
    }

    private void UpdateDeck(List<CardImage> cards)
    {
        // destroy old cards
        if (m_spaw.childCount > 0)
        {
            for (int i = m_spaw.childCount - 1; i >= 0; i--)
            {
                Destroy( m_spaw.GetChild(i).gameObject );
            }            
        }

        // spaw new cards
        foreach (var item in cards)
        {
            CardImage cardImage = Instantiate(item, Vector3.zero, Quaternion.identity, m_spaw);
            cardImage.selectConteriner = selectConteriner;
            cardImage.selectedConteriner = selectedConteriner;
        }
    }

    private void OnDestroy()
    {
        m_deck.OnUpdateHands -= UpdateDeck;
    }
}
