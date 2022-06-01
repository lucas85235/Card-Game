using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI text;

    private void Start()
    {
        ScrapCoins.Instance.OnUpdateCoins.AddListener(UpdateText);
        UpdateText();
    }

    private void UpdateText()
    {
        text.text = ScrapCoins.Instance.TotalCoins.ToString();
    }
}
