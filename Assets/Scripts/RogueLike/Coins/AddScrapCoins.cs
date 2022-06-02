using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddScrapCoins : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void SimpleEnemy()
    {
        int earn = Random.Range(8, 15);
        ScrapCoins.Instance.TotalCoins += earn;
        UpdateText(earn);
    }

    public void BossEnemy()
    {
        int earn = Random.Range(200, 250);
        ScrapCoins.Instance.TotalCoins += earn;
        UpdateText(earn);
    }

    private void UpdateText(int earn)
    {
        text.text = "+" + earn;
    }

}
