using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    public class CardSelect : MonoBehaviour
    {
        private bool isSelected = false;
        private DeckHandle owner;

        private RectTransform thisRect;
        private Vector2 defaultDeltaSize;

        public bool IsSelected => isSelected;

        private void Awake()
        {
            thisRect = GetComponent<RectTransform>();
            defaultDeltaSize = thisRect.sizeDelta;
        }

        public void SetOwner(DeckHandle owner)
        {
            this.owner = owner;
        }

        public void Select()
        {
            if (owner == null) return;
            isSelected = !isSelected;

            if (isSelected)
            {
                thisRect.sizeDelta = (defaultDeltaSize * 1.2f);
                // transform.SetParent(owner.CardsContainer);
            }
            else thisRect.sizeDelta = defaultDeltaSize;
        }
    }
}
