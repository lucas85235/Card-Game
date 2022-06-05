using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    [CreateAssetMenu(fileName = "CardData", menuName = "ScriptableObjects/CardData", order = 1)]
    public class CardData : ScriptableObject
    {
        [Header("Art")]
        public Sprite sprite;

        [Header("Infomation")]
        public Info info;
    }

    [System.Serializable]
    public struct Info
    {
        public CardType type;
        public int value;
    }

    public enum CardType
    {
        Attack,
        Shild,
    }
}
