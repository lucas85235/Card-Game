using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

using System.Threading.Tasks;

namespace Multiplayer
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI roundText;

        [Header("Settings")]
        [SerializeField] private float timeBetweenRounds = 1.5f;
        [SerializeField] private float timeToPlayCards = 1f;
        [SerializeField] private int timeBetweenPlayer = 700;

        [Header("Players")]
        [SerializeField] private RobotMultiplayer playerOne;
        [SerializeField] private RobotMultiplayer playerTwo;
        [SerializeField] private List<RobotMultiplayer> players = new List<RobotMultiplayer>();

        [Header("Alert")]
        [SerializeField] private GameObject alertText;
        [SerializeField] private RectTransform alertLeft;
        [SerializeField] private RectTransform alertRight;

        [Header("Icons")]
        [SerializeField] private List<Icon> iconList;

        [HideInInspector] public UnityEvent OnStartRound;
        [HideInInspector] public UnityEvent OnEndRound;

        private int roundCount = 0;
        private bool gameOver = false;

        private Dictionary<Stats, Dictionary<bool, Sprite>> m_IconDictionary = new Dictionary<Stats, Dictionary<bool, Sprite>>();

        private const int MAX_CARD_PRIORITY = 4;
        public static GameManager Instance;

        private void Awake()
        {
            Instance = this;
            
            foreach (var icon in iconList)
            {
                if (!m_IconDictionary.ContainsKey(icon.stat))
                    m_IconDictionary[icon.stat] = new Dictionary<bool, Sprite>();

                m_IconDictionary[icon.stat][icon.positive] = icon.sprite;
            }
        }

        private void Start()
        {
            SortBySpeed();
        }

        /// <summary>Sort robot attack order according to current speed</summary>
        private void SortBySpeed()
        {
            if (players.Count == 0) players = new List<RobotMultiplayer>(2) { playerOne, playerTwo };

            if (playerOne.CurrentRobotStats[Stats.speed] > playerTwo.CurrentRobotStats[Stats.speed])
            {
                players[0] = playerOne;
                players[1] = playerTwo;
            }
            else if (playerOne.CurrentRobotStats[Stats.speed] < playerTwo.CurrentRobotStats[Stats.speed])
            {
                players[0] = playerTwo;
                players[1] = playerOne;
            }

            else // if equals use coin flip logic
            {
                players[0] = playerOne;
                players[1] = playerTwo;
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            bool allReady = true;

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                var hash = PhotonNetwork.PlayerList[i].CustomProperties;

                if (hash.ContainsKey("EndTurn"))
                {
                    var value = (bool)hash["EndTurn"];
                    if (!value) allReady = false;
                }
            }

            if (allReady)
            {
                EndRound();
            }
        }

        private void StartRound()
        {
            if (gameOver) return;

            roundCount++;
            roundText.text = "Round " + roundCount;
            OnStartRound?.Invoke();

            if (PhotonNetwork.IsMasterClient)
                playerOne.GetComponent<DeckHandle>().SpawCards();
            else playerTwo.GetComponent<DeckHandle>().SpawCards();

            if (!PhotonNetwork.IsMasterClient)
                return;

            // Enable End Turn

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                var hash = PhotonNetwork.PlayerList[i].CustomProperties;

                if (hash.ContainsKey("EndTurn"))
                {
                    hash["EndTurn"] = false;
                    PhotonNetwork.PlayerList[i].SetCustomProperties(hash);
                }
                else
                {
                    hash.Add("EndTurn", false);
                    PhotonNetwork.PlayerList[i].SetCustomProperties(hash);
                }
            }

            Debug.Log("Start Round");
        }

        public void EndRound()
        {
            if (gameOver) return;

            OnEndRound?.Invoke();
            Debug.Log("End Round");

            if (PhotonNetwork.IsMasterClient)
                playerOne.GetComponent<DeckHandle>().SpawSelectedCards();
            else playerTwo.GetComponent<DeckHandle>().SpawSelectedCards();

            Invoke(nameof(PlayCards), timeToPlayCards);
        }

        private async void PlayCards()
        {
            var m_roundCards = new Dictionary<int, List<CardImage>>();
            SortBySpeed();

            for (int i = players.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < players[i].selectedCardsConteriner.childCount; j++)
                {
                    players[i].selectedCardsConteriner.GetChild(j).TryGetComponent(out CardImage cardImage);

                    if (!m_roundCards.ContainsKey(cardImage.Data.Priority))
                    {
                        m_roundCards[cardImage.Data.Priority] = new List<CardImage>();
                    }

                    m_roundCards[cardImage.Data.Priority].Add(cardImage);
                }
            }

            for (int i = 0; i <= MAX_CARD_PRIORITY; i++)
            {
                if (!m_roundCards.ContainsKey(i))
                {
                    continue;
                }

                foreach (var card in m_roundCards[i])
                {
                    // UseCard.Invoke(card);
                    // Robot Attack Feedback Events
                    // RobotAttack.Invoke(card.ConnectedRobot, GameController.i.GetTheOtherRobot(card.ConnectedRobot));

                    card.transform.localScale *= 1.25f;

                    await Task.Delay(timeBetweenPlayer);

                    if (PhotonNetwork.IsMasterClient)
                        card.UseEffect();

                    card.gameObject.SetActive(false);

                    if (card.Data.SingleUse && PhotonNetwork.IsMasterClient)
                    {
                        card.ConnectedRobot.RemoveCard(card.Data);
                    }

                    Destroy(card.gameObject);
                    if (CheckGameOver()) return;
                }
            }

            foreach (var robot in players)
            {
                await robot.ActivateLateStatusEffects(timeBetweenPlayer / 2);
            }

            if (!gameOver) Invoke(nameof(StartRound), timeBetweenRounds);
        }

        private bool CheckGameOver()
        {
            if (playerOne.GetComponent<CharacterLife>().CurrentLife <= 0 || playerTwo.GetComponent<CharacterLife>().CurrentLife <= 0 && !gameOver)
            {
                gameOver = true;

                if (!PhotonNetwork.IsMasterClient)
                    return gameOver;

                PhotonNetwork.LeaveRoom();
            }

            return gameOver;
        }

        public Robot GetTheOtherRobot(Robot emitterRobot)
        {
            foreach (var robot in players)
            {
                if (robot != emitterRobot)
                {
                    return robot;
                }
            }

            return null;
        }

        public void ShowAlertText(int value, bool left, Stats statToShow, Color textColor, string beforeText = "")
        {
            var referenceRect = left ? alertLeft : alertRight;
            int direction;

            var newAlert = Instantiate(alertText, referenceRect);
            newAlert.LeanScaleX(transform.localScale.x > 0 ? 1 : -1, 0);

            var imageObject = newAlert.transform.Find("AlertImage").gameObject;

            if (!m_IconDictionary.ContainsKey(statToShow) || !m_IconDictionary[statToShow].ContainsKey(value > 0))
            {
                Destroy(imageObject);
            }
            else
            {
                imageObject.TryGetComponent(out Image imageComponent);
                imageComponent.sprite = m_IconDictionary[statToShow][value > 0];
            }

            if (value > 0)
            {
                AudioManager.Instance.Play(AudiosList.robotEffect);
                direction = 1;
            }
            else
            {
                AudioManager.Instance.Play(AudiosList.robotDeffect);
                direction = -1;
            }

            newAlert.transform.Find("AlertText").TryGetComponent(out TextMeshProUGUI textComponent);
            textComponent.text = beforeText + value.ToString();
            textComponent.color = textColor;

            newAlert.TryGetComponent(out CanvasGroup textCGroup);
            LeanTween.value(1, 0, 2)
                .setOnUpdate((float value) =>
                {
                    textCGroup.alpha = value;
                });

            newAlert.TryGetComponent(out RectTransform textRect);
            textRect.LeanMoveLocalY(200 * direction, 2)
                .setOnComplete(() =>
                {
                    Destroy(newAlert);
                });
        }
    }
}
