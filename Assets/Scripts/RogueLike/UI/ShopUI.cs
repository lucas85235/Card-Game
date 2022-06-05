using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    [Header("Consumables")]
    public Button buyRepairs;
    public Button buyStickers;
    public Button buySmokes;

    [Header("Upgrades")]
    public Button buyAttack;
    public Button buyDefense;
    public Button buyIntelligence;
    public Button buySpeed;


    void Start()
    {
        buyRepairs.onClick.AddListener(() =>
        {
            if (!CanBuy(20)) return;
            ScrapCoins.Instance.TotalCoins -= 20;
            RogueItems.Instance.Repairs += 1;
        });

        buyStickers.onClick.AddListener(() =>
        {
            if (!CanBuy(10)) return;
            ScrapCoins.Instance.TotalCoins -= 10;
            RogueItems.Instance.Stickers += 1;
        });

        buySmokes.onClick.AddListener(() =>
        {
            if (!CanBuy(20)) return;
            ScrapCoins.Instance.TotalCoins -= 20;
            RogueItems.Instance.Smokes += 1;
        });

        buyAttack.onClick.AddListener(() =>
        {
            if (!CanBuy(10)) return;
            ScrapCoins.Instance.TotalCoins -= 10;
            RogueItems.Instance.Attack += 1;
        });

        buyDefense.onClick.AddListener(() =>
        {
            if (!CanBuy(10)) return;
            ScrapCoins.Instance.TotalCoins -= 10;
            RogueItems.Instance.Defense += 1;
        });

        buyIntelligence.onClick.AddListener(() =>
        {
            if (!CanBuy(10)) return;
            ScrapCoins.Instance.TotalCoins -= 10;
            RogueItems.Instance.Intelligence += 1;
        });
        
        buySpeed.onClick.AddListener(() =>
        {
            if (!CanBuy(10)) return;
            ScrapCoins.Instance.TotalCoins -= 10;
            RogueItems.Instance.Velocity += 1;
        });
    }

    private bool CanBuy(int price)
    {
        return ScrapCoins.Instance.TotalCoins >= price;
    }
}
