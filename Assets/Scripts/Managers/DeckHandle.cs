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

        // chamo o turn a cada começo de turno
        GameController.i.OnStartTurn.AddListener(() => Turn());

        Turn();
    }

    private void SortDeck()
    {
        deck = new List<CardData>();
        discard = new List<CardData>();

        // necessario para resetar o mão ao acabar o deck
        hands = new List<CardData>();

        foreach (var card in robot.data.Cards())
        {
            deck.Add(card);
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

        hands = new List<CardData>();
        var deckSelect = GetRandomHandsList();

        // adiciona a mão
        foreach (var s in deckSelect)
        {
            hands.Add(deck[s]);
        }

        // tira do deck
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
        // chama o evento que atualiza as cartas na mão
        OnUpdateHands(hands);          
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
