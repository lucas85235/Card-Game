using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Multiplayer
{
    public class Card : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private CardData data;

        [Header("Setup")]
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;

        private string objectName;

        public CardData Data
        {
            get => data;
            set
            {
                data = value;
                UpdateData();
            }
        }

        private void UpdateData()
        {
            image.sprite = data.sprite;
            text.text = objectName + " -> " + data.info.type.ToString() + " " + data.info.value;
        }

        public void SetName(string newName)
        {
            objectName = newName;
        }

    }
}