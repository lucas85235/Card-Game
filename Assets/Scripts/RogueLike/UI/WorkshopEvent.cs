using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkshopEvent : MonoBehaviour
{
    [Header("Consumables")]
    public Button buyOne;
    public Button buyTwo;

    private void Start()
    {
        buyOne.interactable = RogueLife.Instance.Life < RogueLife.Instance.MaxLife;
        buyTwo.interactable = RogueLife.Instance.Life < RogueLife.Instance.MaxLife;
        
        buyOne.onClick.AddListener(() =>
        {
            if (!CanBuy(20)) return;
            ScrapCoins.Instance.TotalCoins -= 20;
            RogueLife.Instance.AddLife(20);
            buyOne.interactable = false;
        });

        buyTwo.onClick.AddListener(() =>
        {
            if (!CanBuy(50)) return;
            ScrapCoins.Instance.TotalCoins -= 50;
            RogueLife.Instance.AddLife(RogueLife.Instance.MaxLife);
            buyTwo.interactable = false;
        });
    }

    private void OnEnable()
    {
        if (RogueLife.Instance != null)
        {
            buyOne.interactable = RogueLife.Instance.Life < RogueLife.Instance.MaxLife;
            buyTwo.interactable = RogueLife.Instance.Life < RogueLife.Instance.MaxLife;            
        }
    }

    private void OnDisable()
    {
        buyOne.interactable = true;
        buyTwo.interactable = true;
    }

    private bool CanBuy(int price)
    {
        return ScrapCoins.Instance.TotalCoins >= price;
    }
}
