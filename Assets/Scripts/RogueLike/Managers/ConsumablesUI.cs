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
        RogueConsumables.Instance.OnUpdateRepairs.AddListener(() => {
            repairsText.text = RogueConsumables.Instance.Repairs.ToString();
        });
        RogueConsumables.Instance.OnUpdateStickers.AddListener(() => {
            repairsText.text = RogueConsumables.Instance.Stickers.ToString();
        });
        RogueConsumables.Instance.OnUpdateSmokes.AddListener(() => {
            repairsText.text = RogueConsumables.Instance.Smokes.ToString();
        });
    }
}
