using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConsumablesUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI repairsText;
    public TextMeshProUGUI stickersText;
    public TextMeshProUGUI smokesText;
    
    private void Start()
    {
        if (RogueConsumables.Instance == null) return;
        
        RogueConsumables.Instance.OnUpdateRepairs.AddListener(Repairs);
        RogueConsumables.Instance.OnUpdateStickers.AddListener(Stickers);
        RogueConsumables.Instance.OnUpdateSmokes.AddListener(Smokes);
    }

    private void Repairs()
    {
        repairsText.text = RogueConsumables.Instance.Repairs.ToString();
    }

    private void Stickers()
    {
        stickersText.text = RogueConsumables.Instance.Stickers.ToString();
    }

    private void Smokes()
    {
        smokesText.text = RogueConsumables.Instance.Smokes.ToString();
    }
}
