using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Multiplayer
{
    public class RobotMultiplayer : Robot
    {
        [Header("Custom Settings")]
        public Transform selectedCardsConteriner;
        [SerializeField] private bool getFromDataManager = false;

        public RobotAnimation Animation => m_RobotAnimation;

        private void Awake()
        {
            if (getFromDataManager && DataManager.Instance != null)
            {
                m_Data = DataManager.Instance.GetCurrentRobot();
            }

            TryGetComponent(out m_RobotAnimation);
            Animation.ChangeRobotSprites(m_Data);

            m_iconSpawInLeft = !PhotonNetwork.IsMasterClient;

            CurrentCards = m_Data.Cards();
            SetStats();
        }

        /// <summary>Set Robot Behaviour After Attack Event Called</summary>
        public void RobotAttackFeedback(Robot robot)
        {
            if (robot != this) return;
            Animation.PlayAnimation(Animations.action);
        }
    }
}
