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

        for (int i = 0; i < newParts.Count; i++)
        {
            DataManager.Instance.AddPartItem(newParts[i], "code" + (i + 1));
        }

        DataManager.Instance.AssignPartToRobot("code1", 0);
        DataManager.Instance.AssignPartToRobot("code2", 0);
        DataManager.Instance.AssignPartToRobot("code3", 0);
        DataManager.Instance.AssignPartToRobot("code4", 0);
        DataManager.Instance.AssignPartToRobot("code5", 0);
        DataManager.Instance.AssignPartToRobot("code6", 1);
        DataManager.Instance.AssignPartToRobot("code7", 1);
        DataManager.Instance.AssignPartToRobot("code8", 1);
        DataManager.Instance.AssignPartToRobot("code9", 1);
        DataManager.Instance.AssignPartToRobot("code10", 1);
        DataManager.Instance.AssignPartToRobot("code11", 2);
        DataManager.Instance.AssignPartToRobot("code12", 2);
        DataManager.Instance.AssignPartToRobot("code13", 2);
        DataManager.Instance.AssignPartToRobot("code14", 2);
        DataManager.Instance.AssignPartToRobot("code15", 2);
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
            "Health: " + DataManager.Instance.GetCurrentRobot().Health() +
            " Attack: " + DataManager.Instance.GetCurrentRobot().Attack() +
            " Defence: " + DataManager.Instance.GetCurrentRobot().Defense() +
            " Speed: " + DataManager.Instance.GetCurrentRobot().Speed() +
            " Energy: " + DataManager.Instance.GetCurrentRobot().Energy();

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
