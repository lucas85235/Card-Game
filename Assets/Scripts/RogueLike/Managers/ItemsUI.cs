using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void Start()
    {        
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
        repairsText.text = RogueItems.Instance.Repairs.ToString();
    }

    private void Stickers()
    {
        stickersText.text = RogueItems.Instance.Stickers.ToString();
    }

    private void Smokes()
    {
        smokesText.text = RogueItems.Instance.Smokes.ToString();
    }

    private void Attack()
    {
        attackText.text = RogueItems.Instance.Attack.ToString();
    }

    private void Defense()
    {
        defenseText.text = RogueItems.Instance.Defense.ToString();
    }

    private void Intelligence()
    {
        intelligenceText.text = RogueItems.Instance.Intelligence.ToString();
    }

    private void Velocity()
    {
        velocityText.text = RogueItems.Instance.Velocity.ToString();
    }

}
