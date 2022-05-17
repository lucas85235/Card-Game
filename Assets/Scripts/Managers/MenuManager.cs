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

    [Header("UI")]
    [SerializeField] private GameObject partContainer;
    [SerializeField] private Transform selectedContainer;
    [SerializeField] private Transform headOptionsContainer;
    [SerializeField] private Transform leftArmOptionsContainer;
    [SerializeField] private Transform rightArmOptionsContainer;
    [SerializeField] private Transform torsoOptionsContainer;
    [SerializeField] private Transform legsOptionsContainer;

    private List<Transform> robotPartOrder = new List<Transform>();

    private void Awake()
    {
        SetRobotPartOrder();
        LoadTestData();

        robotAnimation.ChangeRobotSprites(DataManager.Instance.GetCurrentRobot());
    }

    private void SetRobotPartOrder()
    {
        robotPartOrder.Add(headOptionsContainer);
        robotPartOrder.Add(leftArmOptionsContainer);
        robotPartOrder.Add(legsOptionsContainer);
        robotPartOrder.Add(rightArmOptionsContainer);
        robotPartOrder.Add(torsoOptionsContainer);
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

        #region Stunt

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.H_StuntHead.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.LA_StuntLeftArm.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.L_StuntLeg.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.RA_StuntRightArm.ToString()
        });

        newParts.Add(new RobotPartItem()
        {
            itemID = PartID.T_StuntTorso.ToString()
        });

        #endregion

        

        for (int i = 0; i < newParts.Count; i++)
        {
            DataManager.Instance.AddPartItem(newParts[i], "code" + (i + 1));

            GameObject newOption = Instantiate(partContainer, robotPartOrder[i % 5]);
            newOption.TryGetComponent(out PartOptionButton partOption);
            partOption.PartCode = "code" + (i + 1);
            partOption.orderLayer = i % 5;

            newOption.TryGetComponent(out Button optionButton);
            optionButton.onClick.AddListener(() => {
                if(optionButton.transform.parent == selectedContainer)
                {
                    return;
                }

                newOption.TryGetComponent(out PartOptionButton partOption);
                DataManager.Instance.AssignPartToRobot(partOption.PartCode);

                int oldIndex = optionButton.transform.GetSiblingIndex();
                Transform oldSelected = selectedContainer.GetChild(partOption.orderLayer);

                oldSelected.transform.SetParent(robotPartOrder[partOption.orderLayer]);
                oldSelected.transform.SetSiblingIndex(oldIndex);

                optionButton.transform.SetParent(selectedContainer);
                optionButton.transform.SetSiblingIndex(partOption.orderLayer);

                PlayClickSound();
                FillRobotInformation();
            });

            newOption.transform.GetChild(0).GetChild(0).TryGetComponent(out Image buttonImage);
            buttonImage.sprite = DataManager.Instance.GetPartSprite("code" + (i + 1));
        }

        for (int i = 0; i < 5; i++)
        {
            robotPartOrder[i].GetChild(0).transform.SetParent(selectedContainer);
            DataManager.Instance.AssignPartToRobot("code" + (i + 1));
        }
    }

    private void Start()
    {
        FillRobotInformation();

        AudioManager.Instance.Play(AudiosList.menuMusic, isMusic: true);
        AudioManager.Instance.ChangeMusicVolumeWithLerp(1, 1f, startVolume: 0);
    }
    public void ChangeRobot(int value)
    {
        DataManager.Instance.ChangePart(value);

        if (AudioManager.Instance != null)
            AudioManager.Instance.Play(AudiosList.changeRobot);

        FillRobotInformation();
    }

    private void FillRobotInformation()
    {
        robotAnimation.ChangeRobotSprites(DataManager.Instance.GetCurrentRobot());

        robotInfoText.text =
            LanguageManager.Instance.GetKeyValue("health") + ": " + DataManager.Instance.GetCurrentRobot().Health() +
            LanguageManager.Instance.GetKeyValue("attack") + ": " + DataManager.Instance.GetCurrentRobot().Attack() +
            LanguageManager.Instance.GetKeyValue("defense") + ": " + DataManager.Instance.GetCurrentRobot().Defense() +
            LanguageManager.Instance.GetKeyValue("speed") + ": " + DataManager.Instance.GetCurrentRobot().Speed() +
            LanguageManager.Instance.GetKeyValue("energy") + ": " + DataManager.Instance.GetCurrentRobot().Energy();

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
            statsText.text = LanguageManager.Instance.GetKeyValue(card.TitleKey());

            newCardInfo.transform.Find("DescriptionText").TryGetComponent(out TextMeshProUGUI descriptionText);
            descriptionText.text = LanguageManager.Instance.GetKeyValue(card.DescriptionKey());
        }
    }

    public void ReceiveLanguageChange(int value)
    {
        LanguageManager.Instance.LoadLocalizedText(languageIndex: value);
    }
    
    public void StartGame()
    {
        TransitionManager.Instance.StartTransition("Game");
    }

    public void LoadScene(string scene)
    {
        TransitionManager.Instance.StartTransition(scene);
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
