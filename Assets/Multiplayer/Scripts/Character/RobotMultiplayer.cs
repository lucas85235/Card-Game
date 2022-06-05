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
        private PhotonView _view;

        private void Awake()
        {
            _view = GetComponent<PhotonView>();
            
            if (getFromDataManager && DataManager.Instance != null)
            {
                m_Data = DataManager.Instance.GetCurrentRobot();
            }

            TryGetComponent(out m_RobotAnimation);
            Animation.ChangeRobotSprites(m_Data);

            m_iconSpawInLeft = transform.localScale.x > 0;

            CurrentCards = m_Data.Cards();
            SetStats();
        }

        /// <summary>Set Robot Behaviour After Attack Event Called</summary>
        public void RobotAttackFeedback(Robot robot)
        {
            if (robot != this) return;
            Animation.PlayAnimation(Animations.action);
        }

        public override void ApplyStatChange(Stats statToChange, int value)
        {
            if (PhotonNetwork.IsMasterClient)
                _view.RPC(nameof(ApplyStatChangeRPC), RpcTarget.AllBuffered, (int) statToChange, value);
        }

        [PunRPC]
        public void ApplyStatChangeRPC(int statToChange, int value)
        {
            CurrentRobotStats[(Stats) statToChange] += value;
            var textColor = value > 0 ? Color.blue : Color.red;

            if (Multiplayer.GameManager.Instance != null)
                Multiplayer.GameManager.Instance.ShowAlertText(value, m_iconSpawInLeft, (Stats) statToChange, textColor);
        }
    }
}
