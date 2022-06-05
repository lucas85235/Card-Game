using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

namespace Multiplayer
{
    [RequireComponent(typeof(PhotonView))]

    public class CharacterLife : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Slider lifeSlider;
        [SerializeField] private TextMeshProUGUI lifeText;
        [SerializeField] private Slider shildSlider;
        [SerializeField] private TextMeshProUGUI shildText;

        [Space]
        [Header("Events")]
        public UnityEvent OnDie;

        private int _maxLife = 8;
        private bool _shildActive = false;
        private PhotonView _view;
        private RobotMultiplayer _robot;

        private int _currentLife;
        public int CurrentLife
        {
            get => _currentLife;
            private set
            {
                _currentLife = value;
                lifeSlider.value = value;

                if (_currentLife < 1)
                {
                    _currentLife = 0;
                    OnDie?.Invoke();
                }
                else if (_currentLife > _maxLife)
                {
                    _currentLife = _maxLife;
                }

                if (lifeText != null)
                    lifeText.text = _currentLife + " / " + lifeSlider.maxValue;
            }
        }

        private int _currentShild;
        private int CurrentShild
        {
            get => _currentShild;
            set
            {
                _currentShild = value;

                if (_currentShild > shildSlider.maxValue)
                    _currentShild = (int)shildSlider.maxValue;

                if (_currentShild <= 0)
                {
                    _shildActive = false;
                    shildSlider.gameObject.SetActive(false);
                    shildSlider.maxValue = 0;
                    shildSlider.value = 0;
                }

                shildSlider.value = _currentShild;

                if (shildText != null)
                    shildText.text = _currentShild + " / " + shildSlider.maxValue;
            }
        }

        private void Awake()
        {
            _robot = GetComponent<RobotMultiplayer>();
        }

        private void Start()
        {
            _view = GetComponent<PhotonView>();
            lifeSlider.maxValue = _maxLife;
            CurrentLife = _maxLife;

            ResetShild();
            GameManager.Instance.OnStartRound.AddListener(ResetShild);
        }

        public void TakeDamage(int damage)
        {
            _view.RPC(nameof(AnimationRPC), RpcTarget.All, damage);
            _view.RPC(nameof(TakeDamageRPC), RpcTarget.AllBuffered, damage);
        }

        [PunRPC]
        private void AnimationRPC(int damage)
        {
            if (CurrentLife - damage < 1)
            {
                AudioManager.Instance.Play(AudiosList.robotDeath);
                _robot.Animation.PlayAnimation(Animations.death);
                _robot.Animation.ResetToIdleAfterAnimation(false);
            }
            else
            {
                _robot.Animation.PlayAnimation(Animations.hurt);
                AudioManager.Instance.Play(AudiosList.robotHurt);
            }
        }

        [PunRPC]
        private void TakeDamageRPC(int damage)
        {
            if (_shildActive)
            {
                var diff = CurrentShild - damage;
                CurrentShild -= damage;

                if (diff < 0)
                    damage = diff * -1;
                else return;
            }

            CurrentLife -= damage;
        }

        public void AddShild(int damage)
        {
            _view.RPC(nameof(AddShildRPC), RpcTarget.AllBuffered, damage);
        }

        [PunRPC]
        private void AddShildRPC(int increment)
        {
            if (!_shildActive)
            {
                _shildActive = true;
                shildSlider.gameObject.SetActive(true);
            }

            shildSlider.maxValue += increment;
            CurrentShild = (int)shildSlider.maxValue;
        }

        private void ResetShild()
        {
            _view.RPC(nameof(ResetShildRPC), RpcTarget.AllBuffered);
        }

        [PunRPC]
        private void ResetShildRPC()
        {
            _shildActive = false;
            shildSlider.gameObject.SetActive(false);
            shildSlider.maxValue = 0;
            shildSlider.value = 0;
        }
    }
}
