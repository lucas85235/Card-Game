using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class DeckHandle : MonoBehaviour
{
    // o deck possui 20 cartas
    // a cada turno e comprado 5
    // ao final de cada turno as cartas que não forem usadas e as que forem vão para o discard
    // assim que o deck tiver menos que 5 cartas elas setão reenbaralhadas e compradas
    // sera comprado as que tiverem e as que forem reenbaralhadas para completar 5

    // serão as cartas dos personagens
    [Header("Temp")]
    public List<CardImage> mockCards = new List<CardImage>();

    [Header("Lists")]
    [SerializeField] private List<CardImage> deck;
    [SerializeField] private List<CardImage> hands;
    [SerializeField] private List<CardImage> discard;

    [Header("Hud Setup")]
    public Text deckText;
    public Text discardText;

    public Action<List<CardImage>> OnUpdateHands;

    private void Start()
    {
        SortDeck();

        // chamo o turn a cada começo de turno
        GameController.i.OnStartTurn.AddListener(() => Turn());
    }

    private void SortDeck()
    {
        deck = new List<CardImage>();
        discard = new List<CardImage>();

        // necessario para resetar o mão ao acabar o deck
        hands = new List<CardImage>();

        for (int i = 0; i < 4; i++)
        {
            foreach (var mock in mockCards)
            {
                deck.Add(mock);
            }
        }

        // ordena de forma aleatoria o deck
        deck.Sort((a, b) => 1 - 2 * Random.Range(0, 1));
    }

    private void Turn()
    {
        Debug.Log("Turn");

        // adicionar cartas quando não tiver o suficiente
        if (deck.Count < 5)
        {
            SortDeck();
        }

        // adicionar as cartas da mão ao discard
        foreach (var card in hands)
        {
            discard.Add(card);
        }

        hands = new List<CardImage>();
        var deckSelect = GetRandomHandsList();

        // adiciona a mão
        foreach (var s in deckSelect)
        {
            hands.Add(deck[s]);
        }

        // chama o evento que atualiza as cartas na mão
        OnUpdateHands(hands);

        // tira do deck
        foreach (var s in deckSelect)
        {
            deck.RemoveAt(s);
        }

        // debug
        // for (int i = 0; i < hands.Count; i++)
        // {
        //     Debug.Log(deckSelect[i] + " - " + hands[i].name);
        // }

        deckText.text = "Deck: " + deck.Count;
        discardText.text = "Discard: " + discard.Count;
    }

    // seleciona aleatoriamente a ordem da mão atual
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

    private void OnDestroy()
    {
        OnUpdateHands = null;
    }
}
