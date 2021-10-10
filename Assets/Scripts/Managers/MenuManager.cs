using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MenuManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private RobotData[] currentRobots;
    [SerializeField] private RobotAnimation robotAnimation;

    [Header("Info")]
    [SerializeField] private TextMeshProUGUI robotInfoText;
    [SerializeField] private RectTransform cardConfiner;
    [SerializeField] private GameObject cardInfoPrefab;

    private int m_CurrentRobotIndex = 0;

    private void Awake()
    {
        FillRobotInformation();
    }

    private void FillRobotInformation()
    {
        ChangeRobotSprites();

        robotInfoText.text =
            "Health: " + currentRobots[m_CurrentRobotIndex].Health() +
            " Attack: " + currentRobots[m_CurrentRobotIndex].Attack() +
            " Defence: " + currentRobots[m_CurrentRobotIndex].Defense() +
            " Speed: " + currentRobots[m_CurrentRobotIndex].Speed() +
            " Energy: " + currentRobots[m_CurrentRobotIndex].Energy();


        foreach (RectTransform oldCard in cardConfiner)
            Destroy(oldCard.gameObject);
        

        foreach (var card in currentRobots[m_CurrentRobotIndex].Cards())
        {
            var newCardInfo = Instantiate(cardInfoPrefab);
            newCardInfo.transform.SetParent(cardConfiner, false);

            newCardInfo.transform.Find("CardSprite").TryGetComponent(out Image cardImage);
            cardImage.sprite = card.Sprite();

            newCardInfo.transform.Find("StatsText").TryGetComponent(out TextMeshProUGUI statsText);
            statsText.text = card.Title();

            newCardInfo.transform.Find("DescriptionText").TryGetComponent(out TextMeshProUGUI descriptionText);
            descriptionText.text = card.Description();
        }
    }

    private void ChangeRobotSprites()
    {
        //Retorna sprites na ordem: Tronco - Pernas - Cabeça - Ombro Direito - Braço Direito - Ombro Esquerdo - Braço Esquerdo
        var newSprites = currentRobots[m_CurrentRobotIndex].Sprites();

        robotAnimation.TorsoSpriteRen().sprite = newSprites[0];
        robotAnimation.LegsSpriteRen().sprite = newSprites[1];
        robotAnimation.HeadSpriteRen().sprite = newSprites[2];
        robotAnimation.RightShoulderSpriteRen().sprite = newSprites[3];
        robotAnimation.RightArmSpriteRen().sprite = newSprites[4];
        robotAnimation.LeftShoulderSpriteRen().sprite = newSprites[5];
        robotAnimation.LeftArmSpriteRen().sprite = newSprites[6];
    }

    public void ChangeRobot(int value)
    {
        m_CurrentRobotIndex += value;

        if (m_CurrentRobotIndex < 0) m_CurrentRobotIndex = currentRobots.Length - 1;
        if (m_CurrentRobotIndex > currentRobots.Length - 1) m_CurrentRobotIndex = 0;

        FillRobotInformation();
    }
}
