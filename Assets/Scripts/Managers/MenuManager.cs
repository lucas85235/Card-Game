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
    [SerializeField] private RectTransform cardMenuConfiner;
    [SerializeField] private RectTransform cardPartConfiner;
    [SerializeField] private GameObject cardInfoPrefab;

    [Header("UI")]
    [SerializeField] private GameObject partItem;
    [SerializeField] private RectTransform selectedContainer;

    [Header("UI Hand Cards")]
    [SerializeField] private RectTransform handConfiner;

    private List<RobotPartItem> newParts = new List<RobotPartItem>();

    private void Awake()
    {
        LoadTestData();
        
    }

    private void Start()
    {
        robotAnimation.ChangeRobotSprites(DataManager.Instance.GetCurrentRobot());
        
        FillRobotInformation();
        FillRobotPartInformation((int)RobotParts.Head);

        AudioManager.Instance.Play(AudiosList.menuMusic, isMusic: true);
        AudioManager.Instance.ChangeMusicVolumeWithLerp(1, 1f, startVolume: 0);
    }

    private void LoadTestData()
    {
        newParts = new List<RobotPartItem>();
        int countParts = 0;

        foreach (var item in Enum.GetValues(typeof(PartID)))
        {
            newParts.Add(new RobotPartItem()
            {
                itemID = item.ToString()
            });

            countParts++;
        }

        for (int i = 0; i < newParts.Count; i++)
        {
            DataManager.Instance.AddPartItem(newParts[i], "code" + (i + 1));
        }
    }

    private void FillPartsInformation(RobotParts part)
    {
        foreach (RectTransform oldCard in selectedContainer)
            Destroy(oldCard.gameObject);

        for (int i = 0; i < newParts.Count; i++)
        {
            if (i % 5 != (int)part) continue;

            GameObject newOption = Instantiate(partItem, selectedContainer);
            newOption.TryGetComponent(out PartOptionButton partOption);
            partOption.PartCode = "code" + (i + 1);
            partOption.orderLayer = i % 5;

            newOption.TryGetComponent(out Button optionButton);
            optionButton.onClick.AddListener(() =>
            {
                Debug.Log("Select Part");

                newOption.TryGetComponent(out PartOptionButton partOption);
                DataManager.Instance.AssignPartToRobot(partOption.PartCode);
                DataManager.Instance.SavePart(partOption.PartCode, partOption.orderLayer);

                // Select Feedback
                newOption.transform.GetChild(0).TryGetComponent(out Image buttonSelect);
                buttonSelect.color = Color.green;

                PlayClickSound();
                FillRobotInformation();
                FillRobotPartInformation((int)part);
            });

            newOption.transform.GetChild(0).TryGetComponent(out Image buttonSelect);
            if (DataManager.Instance.SaveCodes.Contains("code" + (i + 1)))
            {
                buttonSelect.color = Color.green;
            }
            else buttonSelect.color = Color.white;

            newOption.transform.GetChild(0).GetChild(0).TryGetComponent(out Image buttonImage);
            buttonImage.sprite = DataManager.Instance.GetPartSprite("code" + (i + 1));
        }
    }

    public void ChangeRobot(int value)
    {
        DataManager.Instance.ChangePart(value);

        if (AudioManager.Instance != null)
            AudioManager.Instance.Play(AudiosList.changeRobot);

        FillRobotInformation();
        FillRobotPartInformation((int)RobotParts.Head);
    }
    private void FillRobotInformation()
    {
        FillRobotInformation(cardConfiner);
        FillRobotInformation(cardMenuConfiner);
    }

    private void FillRobotInformation(RectTransform confiner)
    {
        robotAnimation.ChangeRobotSprites(DataManager.Instance.GetCurrentRobot());

        robotInfoText.text =
            LanguageManager.Instance.GetKeyValue("health") + ": " + DataManager.Instance.GetCurrentRobot().Health() + "\n" +
            LanguageManager.Instance.GetKeyValue("attack") + ": " + DataManager.Instance.GetCurrentRobot().Attack() + "\n" +
            LanguageManager.Instance.GetKeyValue("defense") + ": " + DataManager.Instance.GetCurrentRobot().Defense() + "\n" +
            LanguageManager.Instance.GetKeyValue("inteligence") + ": " + DataManager.Instance.GetCurrentRobot().Inteligence() + "\n" +
            LanguageManager.Instance.GetKeyValue("speed") + ": " + DataManager.Instance.GetCurrentRobot().Speed() + "\n" +
            LanguageManager.Instance.GetKeyValue("accuracy") + ": " + DataManager.Instance.GetCurrentRobot().Accuracy() + "\n" +
            LanguageManager.Instance.GetKeyValue("evasion") + ": " + DataManager.Instance.GetCurrentRobot().Evasion() + "\n" +
            LanguageManager.Instance.GetKeyValue("critChance") + ": " + DataManager.Instance.GetCurrentRobot().CritChance() + "\n" +
            LanguageManager.Instance.GetKeyValue("energy") + ": " + DataManager.Instance.GetCurrentRobot().Energy() + "\n" +
            LanguageManager.Instance.GetKeyValue("fireResistence") + ": " + DataManager.Instance.GetCurrentRobot().FireResistence() + "\n" +
            LanguageManager.Instance.GetKeyValue("waterResistence") + ": " + DataManager.Instance.GetCurrentRobot().WaterResistence() + "\n" +
            LanguageManager.Instance.GetKeyValue("electricResistence") + ": " + DataManager.Instance.GetCurrentRobot().ElectricResistence() + "\n" +
            LanguageManager.Instance.GetKeyValue("acidResistence") + ": " + DataManager.Instance.GetCurrentRobot().AcidResistence() + "\n";

        nameInfoText.text =
            DataManager.Instance.GetCurrentRobot().characterName + " - " +
            DataManager.Instance.GetCurrentRobot().botFunction;

        descriptionInfoText.text =
            DataManager.Instance.GetCurrentRobot().storyDescription;

        foreach (RectTransform oldCard in confiner)
            Destroy(oldCard.gameObject);

        foreach (var card in DataManager.Instance.GetCurrentRobot().Cards())
        {
            var newCardInfo = Instantiate(cardInfoPrefab);
            newCardInfo.transform.SetParent(confiner, false);

            newCardInfo.transform.Find("CardSprite").TryGetComponent(out Image cardImage);
            cardImage.sprite = card.Sprite();

            newCardInfo.transform.Find("EnergyText").TryGetComponent(out TextMeshProUGUI energyText);
            energyText.text = card.Energy().ToString();

            newCardInfo.transform.Find("TitleText").TryGetComponent(out TextMeshProUGUI titleText);
            titleText.text = LanguageManager.Instance.GetKeyValue(card.TitleKey());

            newCardInfo.transform.Find("DescriptionText").TryGetComponent(out TextMeshProUGUI descriptionText);
            descriptionText.text = LanguageManager.Instance.GetKeyValue(card.DescriptionKey());
        }

        ReRool();
    }

    public void FillRobotPartInformation(int part)
    {
        FillPartsInformation((RobotParts)part);

        var robot = DataManager.Instance.GetCurrentRobot();

        switch ((RobotParts)part)
        {
            case RobotParts.Head:
                FillRobotPartInformation(robot.GetHead().Cards());
                break;

            case RobotParts.Torso:
                FillRobotPartInformation(robot.GetTorso().Cards());
                break;

            case RobotParts.LeftArm:
                FillRobotPartInformation(robot.GetLeftArm().Cards());
                break;

            case RobotParts.RightArm:
                FillRobotPartInformation(robot.GetRightArm().Cards());
                break;

            case RobotParts.Legs:
                FillRobotPartInformation(robot.GetLeg().Cards());
                break;
        }
    }

    private void FillRobotPartInformation(List<CardData> cards)
    {
        foreach (RectTransform oldCard in cardPartConfiner)
            Destroy(oldCard.gameObject);

        foreach (var card in cards)
        {
            var newCardInfo = Instantiate(cardInfoPrefab);
            newCardInfo.transform.SetParent(cardPartConfiner, false);

            newCardInfo.transform.Find("CardSprite").TryGetComponent(out Image cardImage);
            cardImage.sprite = card.Sprite();

            newCardInfo.transform.Find("EnergyText").TryGetComponent(out TextMeshProUGUI energyText);
            energyText.text = card.Energy().ToString();

            newCardInfo.transform.Find("TitleText").TryGetComponent(out TextMeshProUGUI titleText);
            titleText.text = LanguageManager.Instance.GetKeyValue(card.TitleKey());

            newCardInfo.transform.Find("DescriptionText").TryGetComponent(out TextMeshProUGUI descriptionText);
            descriptionText.text = LanguageManager.Instance.GetKeyValue(card.DescriptionKey());
        }
    }

    public void ReRool()
    {
        foreach (RectTransform oldCard in handConfiner)
            Destroy(oldCard.gameObject);

        foreach (var card in GetRandomHandsList())
        {
            var newCardInfo = Instantiate(cardInfoPrefab);
            newCardInfo.transform.SetParent(handConfiner, false);

            newCardInfo.transform.Find("CardSprite").TryGetComponent(out Image cardImage);
            cardImage.sprite = card.Sprite();

            newCardInfo.transform.Find("EnergyText").TryGetComponent(out TextMeshProUGUI energyText);
            energyText.text = card.Energy().ToString();

            newCardInfo.transform.Find("TitleText").TryGetComponent(out TextMeshProUGUI titleText);
            titleText.text = LanguageManager.Instance.GetKeyValue(card.TitleKey());

            newCardInfo.transform.Find("DescriptionText").TryGetComponent(out TextMeshProUGUI descriptionText);
            descriptionText.text = LanguageManager.Instance.GetKeyValue(card.DescriptionKey());
        }
    }

    private List<CardData> GetRandomHandsList()
    {
        var deck = new List<CardData>();
        var hands = new List<CardData>();

        foreach (CardData card in DataManager.Instance.GetCurrentRobot().Cards())
        {
            deck.Add(card);
        }

        // randomly order the deck
        deck.Sort((a, b) => 1 - 2 * UnityEngine.Random.Range(0, 1));

        var deckSelect = new List<int>();
        while (deckSelect.Count < 5)
        {
            var r = UnityEngine.Random.Range(0, deck.Count);

            if (!deckSelect.Contains(r))
                deckSelect.Add(r);
        }

        deckSelect.Sort();
        deckSelect.Reverse();

        foreach (var s in deckSelect)
        {
            hands.Add(deck[s]);
        }

        return hands;
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
