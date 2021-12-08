using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MenuManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private RobotAnimation robotAnimation;

    [Header("Info")]
    [SerializeField] private TextMeshProUGUI nameInfoText;
    [SerializeField] private TextMeshProUGUI descriptionInfoText;
    [SerializeField] private TextMeshProUGUI robotInfoText;
    [SerializeField] private RectTransform cardConfiner;
    [SerializeField] private GameObject cardInfoPrefab;

    private int m_CurrentRobotIndex = 0;

    private void Awake()
    {
        LoadTestData();

        FillRobotInformation();

        DataManager.Instance.PlayerInfo.CurrentRobotIndex = m_CurrentRobotIndex;
        robotAnimation.ChangeRobotSprites(DataManager.Instance.GetCurrentRobot());
    }

    private void LoadTestData()
    {
        var newParts = new List<RobotPartItem>();

        #region builder

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.H_BuilderHead.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.LA_BuilderLeftArm.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.L_BuilderLeg.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.RA_BuilderRightArm.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.T_BuilderTorso.ToString()
        });

        #endregion

        #region Elec

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.H_ElecHead.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.LA_ElecLeftArm.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.L_ElecLeg.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.RA_ElecRightArm.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.T_ElecTorso.ToString()
        });

        #endregion

        #region Stun

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.H_StunHead.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.LA_StunLeftArm.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.L_StunLeg.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.RA_StunRightArm.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.T_StunTorso.ToString()
        });

        #endregion

        #region Lumber

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.H_LumberHead.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.LA_LumberLeftArm.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.L_LumberLeg.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.RA_LumberRightArm.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.T_LumberTorso.ToString()
        });

        #endregion

        #region Metal

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.H_MetalHead.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.LA_MetalLeftArm.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.L_MetalLeg.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.RA_MetalRightArm.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.T_MetalTorso.ToString()
        });

        #endregion

        for (int i = 0; i < newParts.Count; i++)
        {
            DataManager.Instance.AddPartItem(newParts[i], "code" + (i + 1));
            DataManager.Instance.AssignPartToRobot("code" + (i + 1), Mathf.FloorToInt(i / 5));
        }
    }

    private void Start()
    {
        AudioManager.Instance.Play(AudiosList.menuMusic, isMusic: true);
        AudioManager.Instance.ChangeMusicVolumeWithLerp(1, 1f, startVolume: 0);
    }

    private void FillRobotInformation()
    {
        DataManager.Instance.PlayerInfo.CurrentRobotIndex = m_CurrentRobotIndex;
        robotAnimation.ChangeRobotSprites(DataManager.Instance.GetCurrentRobot());

        robotInfoText.text =
            "Vida: " + DataManager.Instance.GetCurrentRobot().Health() +
            " Ataque: " + DataManager.Instance.GetCurrentRobot().Attack() +
            " Defesa: " + DataManager.Instance.GetCurrentRobot().Defense() +
            " Velocidade: " + DataManager.Instance.GetCurrentRobot().Speed() +
            " Energia: " + DataManager.Instance.GetCurrentRobot().Energy();

        nameInfoText.text =
            DataManager.Instance.GetCurrentRobot().characterName + " - " +
            DataManager.Instance.GetCurrentRobot().botFunction;

        descriptionInfoText.text =
            DataManager.Instance.GetCurrentRobot().storyDescription;

        foreach (RectTransform oldCard in cardConfiner)
            Destroy(oldCard.gameObject);
        

        foreach (var card in DataManager.Instance.GetCurrentRobot().Cards())
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

    public void StartGame()
    {
        TransitionManager.Instance.StartTransition("Game");
    }

    public void ChangeRobot(int value)
    {
        m_CurrentRobotIndex += value;
        AudioManager.Instance.Play(AudiosList.changeRobot);

        if (m_CurrentRobotIndex < 0) m_CurrentRobotIndex = DataManager.Instance.PlayerInfo.Robots.Length - 1;
        if (m_CurrentRobotIndex > DataManager.Instance.PlayerInfo.Robots.Length - 1) m_CurrentRobotIndex = 0;

        FillRobotInformation();
    }

    public void PlayClickSound()
    {
        AudioManager.Instance.Play(AudiosList.uiClick);
    }

    public void ChangeGeralVolume(float newValue)
    {
        AudioManager.Instance.ChangeGeralVolume(newValue);
    }
    public void ChangeMusicVolume(float newValue)
    {
        AudioManager.Instance.ChangeMusicVolume(newValue);
    }
    public void ChangeSFXVolume(float newValue)
    {
        AudioManager.Instance.ChangeSFXVolume(newValue);
    }
}
