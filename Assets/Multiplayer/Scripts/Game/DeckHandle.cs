using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Multiplayer
{
    public class DeckHandle : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private RectTransform cardsContainer;
        [SerializeField] private GameObject cardTemplate;

        [Header("Deck")]
        [SerializeField] private List<CardData> deck = new List<CardData>();
        [SerializeField] private List<CardImage> selectedCards = new List<CardImage>();

        private Energy _energy;
        private Robot _connectedRobot;

        public RectTransform CardsContainer
        {
            get => cardsContainer;
            set => cardsContainer = value;
        }

        private PhotonView _view;

        private void Awake()
        {
            _energy = GetComponent<Energy>();
            _connectedRobot = GetComponent<Robot>();
        }

        private void OnEnable()
        {
            _view = GetComponent<PhotonView>();
        }

        public List<CardImage> GetRoundCards()
        {
            return selectedCards;
        }

        public void SpawCards()
        {
            selectedCards = new List<CardImage>();

            for (int i = 0; i < deck.Count; i++)
            {
                var value = Random.Range(0f, 1f) > .5f;
                if (!value) continue;

                var cardObj = Instantiate(cardTemplate, cardsContainer);

                if (cardObj.TryGetComponent(out CardImage spawCard))
                {
                    spawCard.energyCount = _energy;
                    spawCard.ConnectedRobot = _connectedRobot;
                    spawCard.selectConteriner = cardsContainer;
                    spawCard.description.text = gameObject.name;
                    spawCard.Data = deck[i];

                    selectedCards.Add(spawCard);
                }

                if (cardObj.TryGetComponent(out CardSelect select))
                {
                    select.SetOwner(this);
                }
            }
        }

        public void SpawSelectedCards()
        {
            // Destroy not selected cards

            for (int i = selectedCards.Count - 1; i >= 0; i--)
            {
                if (selectedCards[i].TryGetComponent(out CardSelect select) && !select.IsSelected)
                {
                    var temp = selectedCards[i];
                    selectedCards.RemoveAt(i);
                    Destroy(temp.gameObject);
                }
            }

            // Save selected cards to spaw

            bool[] rand = new bool[deck.Count];

            for (int j = 0; j < deck.Count; j++)
            {
                for (int i = 0; i < selectedCards.Count; i++)
                {
                    if (selectedCards[i].Data == deck[j])
                        rand[j] = true;

                }
            }

            // Destroy selected cards

            selectedCards = null;

            foreach (RectTransform item in cardsContainer)
                Destroy(item.gameObject);

            _view.RPC(nameof(SpawSelectedCardsRPC), RpcTarget.All, rand);
        }

        [PunRPC]
        private void SpawSelectedCardsRPC(bool[] rand)
        {
            selectedCards = new List<CardImage>();

            for (int i = 0; i < rand.Length; i++)
            {
                if (!rand[i]) continue;

                var cardObj = Instantiate(cardTemplate, cardsContainer);

                if (cardObj.TryGetComponent(out CardImage spawCard))
                {
                    spawCard.energyCount = _energy;
                    spawCard.ConnectedRobot = _connectedRobot;
                    spawCard.selectConteriner = cardsContainer;
                    spawCard.description.text = gameObject.name;
                    spawCard.Data = deck[i];

                    selectedCards.Add(spawCard);
                }
            }
        }
    }
}
