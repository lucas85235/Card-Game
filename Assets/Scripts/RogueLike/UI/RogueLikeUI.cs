using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RogueLikeUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI robotText;
    public TextMeshProUGUI lifeText;

    public Slider lifeSlider;

    private void Start()
    {
        RogueLife.Instance.OnUpdateMaxLife.AddListener(UpdateMaxLifeUI);
        RogueLife.Instance.OnUpdateLife.AddListener(UpdateLifeUI);

        robotText.text = "Robot Name";
    }

    public void UpdateLifeUI()
    {
        lifeSlider.value = RogueLife.Instance.Life;
        lifeText.text = RogueLife.Instance.Life + "/" + RogueLife.Instance.MaxLife;
    }

    public void UpdateMaxLifeUI()
    {
        lifeSlider.maxValue = RogueLife.Instance.MaxLife;
    }
}
