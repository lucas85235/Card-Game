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
        [SerializeField] private List<GameObject> players = new List<GameObject>();

        [HideInInspector] public UnityEvent OnStartRound;
        [HideInInspector] public UnityEvent OnEndRound;

        private int roundCount = 0;
        private bool gameOver = false;

        private DeckHandle playerOneDeck;
        private DeckHandle playerTwoDeck;
        private CharacterLife playerOneLife;
        private CharacterLife playerTwoLife;
        private RobotMultiplayer playerOneMultiplayer;
        private RobotMultiplayer playerTwoMultiplayer;

        public static GameManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            playerOneLife = players[0].GetComponent<CharacterLife>();
            playerTwoLife = players[1].GetComponent<CharacterLife>();
            playerOneDeck = players[0].GetComponent<DeckHandle>();
            playerTwoDeck = players[1].GetComponent<DeckHandle>();
            playerOneMultiplayer = players[0].GetComponent<RobotMultiplayer>();
            playerTwoMultiplayer = players[1].GetComponent<RobotMultiplayer>();
            StartRound();
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
                playerOneDeck.SpawCards();
            else playerTwoDeck.SpawCards();

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

        private void EndRound()
        {
            if (gameOver) return;

            OnEndRound?.Invoke();
            Debug.Log("End Round");

            if (PhotonNetwork.IsMasterClient)
                playerOneDeck.SpawSelectedCards();
            else playerTwoDeck.SpawSelectedCards();

            Invoke(nameof(PlayCards), timeToPlayCards);
        }

        private async void PlayCards()
        {
            var p1_Deck = players[0].GetComponent<DeckHandle>().GetRoundCards();
            var p2_Deck = players[1].GetComponent<DeckHandle>().GetRoundCards();

            // Apply Shilds

            for (int i = p1_Deck.Count - 1; i >= 0; i--)
            {
                if (p1_Deck[i].Data.info.type == CardType.Shild)
                {
                    playerOneMultiplayer.Animation.PlayAnimation(Animations.action);
                    p1_Deck[i].transform.localScale = Vector3.one * 1.25f;
                    await Task.Delay(timeBetweenPlayer);

                    if (PhotonNetwork.IsMasterClient)
                        playerOneLife.AddShild(p1_Deck[i].Data.info.value);

                    var temp = p1_Deck[i];
                    p1_Deck.RemoveAt(i);
                    Destroy(temp.gameObject);
                }
            }

            for (int i = p2_Deck.Count - 1; i >= 0; i--)
            {
                if (p2_Deck[i].Data.info.type == CardType.Shild)
                {
                    playerTwoMultiplayer.Animation.PlayAnimation(Animations.action);
                    p2_Deck[i].transform.localScale = Vector3.one * 1.25f;
                    await Task.Delay(timeBetweenPlayer);

                    if (PhotonNetwork.IsMasterClient)
                        playerTwoLife.AddShild(p2_Deck[i].Data.info.value);

                    var temp = p2_Deck[i];
                    p2_Deck.RemoveAt(i);
                    Destroy(temp.gameObject);
                }
            }

            // Start Attacks

            for (int i = p1_Deck.Count - 1; i >= 0; i--)
            {
                if (p1_Deck[i].Data.info.type == CardType.Attack)
                {
                    playerOneMultiplayer.Animation.PlayAnimation(Animations.action);
                    p1_Deck[i].transform.localScale = Vector3.one * 1.25f;
                    await Task.Delay(timeBetweenPlayer);

                    if (PhotonNetwork.IsMasterClient)
                        playerTwoLife.TakeDamage(p1_Deck[i].Data.info.value);

                    var temp = p1_Deck[i];
                    p1_Deck.RemoveAt(i);
                    Destroy(temp.gameObject);

                    if (CheckGameOver()) return;
                }
            }

            for (int i = p2_Deck.Count - 1; i >= 0; i--)
            {
                if (p2_Deck[i].Data.info.type == CardType.Attack)
                {
                    playerTwoMultiplayer.Animation.PlayAnimation(Animations.action);
                    p2_Deck[i].transform.localScale = Vector3.one * 1.25f;
                    await Task.Delay(timeBetweenPlayer);

                    if (PhotonNetwork.IsMasterClient)
                        playerOneLife.TakeDamage(p2_Deck[i].Data.info.value);

                    var temp = p2_Deck[i];
                    p2_Deck.RemoveAt(i);
                    Destroy(temp.gameObject);

                    if (CheckGameOver()) return;
                }
            }

            if (!gameOver) Invoke(nameof(StartRound), timeBetweenRounds);
        }

        private bool CheckGameOver()
        {
            if (playerOneLife.CurrentLife <= 0 || playerTwoLife.CurrentLife <= 0 && !gameOver)
            {
                gameOver = true;

                if (!PhotonNetwork.IsMasterClient)
                    return gameOver;

                PhotonNetwork.LeaveRoom();
            }

            return gameOver;
        }
    }
}
