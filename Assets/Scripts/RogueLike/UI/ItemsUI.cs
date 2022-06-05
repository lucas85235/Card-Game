using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemsUI : MonoBehaviour
{
    [Header("Consumables")]
    public TextMeshProUGUI repairsText;
    public TextMeshProUGUI stickersText;
    public TextMeshProUGUI smokesText;

    [Header("Upgrades")]
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI intelligenceText;
    public TextMeshProUGUI velocityText;

    [Header("Buttons")]
    public Button useRepairs;
    public Button useStickers;

    private void Start()
    {
        if (useRepairs != null)
        {
            useRepairs.onClick.AddListener(() => {
                RogueLife.Instance.AddLife(20);
                RogueItems.Instance.Repairs -= 1;
            });
        }

        if (useStickers != null)
        {
            useStickers.onClick.AddListener(() => {
                RogueLife.Instance.AddLife(10);
                RogueItems.Instance.Stickers -= 1;
            });            
        }

        RogueItems.Instance.OnUpdateRepairs.AddListener(Repairs);
        RogueItems.Instance.OnUpdateStickers.AddListener(Stickers);
        RogueItems.Instance.OnUpdateSmokes.AddListener(Smokes);

        RogueItems.Instance.OnUpdateAttack.AddListener(Attack);
        RogueItems.Instance.OnUpdateDefense.AddListener(Defense);
        RogueItems.Instance.OnUpdateIntelligence.AddListener(Intelligence);
        RogueItems.Instance.OnUpdateVelocity.AddListener(Velocity);

        Repairs();
        Stickers();
        Smokes();
        
        Attack();
        Defense();
        Intelligence();
        Velocity();
    }

    private void Repairs()
    {
        if (useRepairs != null)
            useRepairs.interactable = RogueItems.Instance.Repairs > 0 && RogueLife.Instance.Life < RogueLife.Instance.MaxLife;
            
        if (repairsText != null)
            repairsText.text = RogueItems.Instance.Repairs.ToString();
    }

    private void Stickers()
    {
        if (useStickers != null)
            useStickers.interactable = RogueItems.Instance.Stickers > 0 && RogueLife.Instance.Life < RogueLife.Instance.MaxLife;

        if (stickersText != null)
            stickersText.text = RogueItems.Instance.Stickers.ToString();
    }

    private void Smokes()
    {
        if (smokesText != null)
            smokesText.text = RogueItems.Instance.Smokes.ToString();
    }

    private void Attack()
    {
        if (attackText != null)
            attackText.text = RogueItems.Instance.Attack.ToString();
    }

    private void Defense()
    {
        if (defenseText != null)
            defenseText.text = RogueItems.Instance.Defense.ToString();
    }

    private void Intelligence()
    {
        if (intelligenceText != null)
            intelligenceText.text = RogueItems.Instance.Intelligence.ToString();
    }

    private void Velocity()
    {
        if (velocityText != null)
            velocityText.text = RogueItems.Instance.Velocity.ToString();
    }

}
