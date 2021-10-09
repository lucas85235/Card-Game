using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckHandle : MonoBehaviour
{

    // o deck possui 20 cartas
    // a cada turno e comprado 5
    // ao final de cada turno as cartas que não forem usadas e as que forem vão para o discard
    // assim que o deck tiver menos que 5 cartas elas setão reenbaralhadas e compradas
    // sera comprado as que tiverem e as que forem reenbaralhadas para completar 5

    // serão as cartas dos personagens
    public List<CardImage> mockCards = new List<CardImage>();

    public List<CardImage> deck;
    public List<CardImage> hands;

    private void Start()
    {
        SortDeck();

        // chamo o turn a cada começo de turno
        GameController.i.OnStartTurn.AddListener(() => Turn());
    }

    public void SortDeck()
    {
        deck = new List<CardImage>();

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

    public void Turn()
    {
        Debug.Log("Turn");

        // adicionar cartas quando não tiver o suficiente
        if (deck.Count < 5) 
        {
            SortDeck();
        }

        hands = new List<CardImage>();
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

        // debug
        for (int i = 0; i < hands.Count; i++)
        {
            Debug.Log(deckSelect[i] + " - " + hands[i].name);
        }
    }

    // seleciona aleatoriamente a ordem da mão atual
    public List<int> GetRandomHandsList()
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
