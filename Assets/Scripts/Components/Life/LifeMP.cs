using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LifeMP : Life
{
    [Header("Death Screens")]
    public GameObject win;
    public GameObject lose;

    [Header("Life Settings")]
    public Slider lifeSlider;
    public TextMeshProUGUI lifeText;

    [Header("Shild Settings")]
    public Slider shildSlider;
    public TextMeshProUGUI shildText;

    private PhotonView m_view;

    private void Awake() 
    {
        m_RobotAnimation = GetComponent<RobotAnimation>();    
    }

    protected override void Start()
    {
        m_robot = GetComponent<Robot>();
        m_view = GetComponent<PhotonView>();

        if (m_view.IsMine)
        {
            var health = m_robot.DataStats[Stats.health];
            m_view.RPC("LifeSetupRPC", RpcTarget.AllBuffered, health);
        }
    }

    protected void Update()
    {
        if (!m_view.IsMine) return;

        if (m_view.IsMine && Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakeDamage(4);
        }
    }

    [PunRPC]
    private void LifeSetupRPC(int health)
    {
        m_maxLife = health;
        m_currentLife = m_maxLife;
        lifeSlider.maxValue = m_maxLife;
        lifeSlider.value = m_maxLife;
        lifeText.text = m_currentLife + " / " + m_maxLife;
    }

    public override void AddLife(int increment)
    {
        m_view.RPC("TakeDamageRPC", RpcTarget.AllBuffered, -increment, null);
    }

    public override void TakeDamage(int damage = 1, params object[] objects)
    {
        m_view.RPC("TakeDamageRPC", RpcTarget.AllBuffered, damage, null);
    }

    [PunRPC]
    private void TakeDamageRPC(int damage = 1, params object[] objects)
    {
        if (m_currentShield > 0)
        {
            m_currentShield -= damage;

            if (m_currentShield <= 0)
            {
                shildSlider.gameObject.SetActive(false);
                Debug.Log("Damage diff: " + m_currentShield * -1);
                damage = m_currentShield * -1;
            }
            else 
            {
                shildSlider.value = m_currentShield;
                shildText.text = m_currentShield + " / " + shildSlider.maxValue;
                damage = 0;
            }
        }

        m_currentLife -= damage;

        // Rules
        if (m_currentLife > m_maxLife)
            m_currentLife = m_maxLife;
        
        if (m_currentLife < 1)
        {
            AudioManager.Instance.Play(AudiosList.robotDeath);
            m_RobotAnimation.PlayAnimation(Animations.death);
            m_RobotAnimation.ResetToIdleAfterAnimation(false);
            DeathHandle();
        }
        else
        {
            m_RobotAnimation.PlayAnimation(Animations.hurt);
            AudioManager.Instance.Play(AudiosList.robotHurt);
        }

        // Update Hud
        lifeSlider.value = m_currentLife;
        lifeText.text = m_currentLife + " / " + m_maxLife;
    }

    protected override void DeathHandle()
    {
        Debug.Log(m_view.Owner.NickName + " Is Dead!");

        if (destroyAfterDeath)
            Destroy(this.gameObject);

        OnDeath?.Invoke();
        isDead = true;

        m_view.RPC("GameOver", RpcTarget.AllViaServer);
    }

    [PunRPC]
    public void GameOver()
    {
        if (m_view.IsMine)
        {
            Debug.Log("Loser: " + m_view.Owner.NickName);
            lose.SetActive(true);
        }
        else
        {
            Debug.Log("Winner: " + m_view.Owner.NickName);
            win.SetActive(true);
        }
    }

    // HUD UPDATE LIFE

    public void UpdateLifeSlider()
    {
        m_view.RPC("UpdateLifeSliderRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void UpdateLifeSliderRPC()
    {
        lifeSlider.value = m_currentLife;
        lifeText.text = m_currentLife + " / " + m_maxLife;
    }

    // SHIELD

    public override void AddShield(int shild)
    {
        m_view.RPC("AddShieldRPC", RpcTarget.AllBuffered, shild);
    }

    [PunRPC]
    private void AddShieldRPC(int shild)
    {
        // GameController.i.ShowAlertText(shild, Color.white, transform.localScale.x > 0);
        m_currentShield += shild;

        AudioManager.Instance.Play(AudiosList.robotEffect);
        shildSlider.gameObject.SetActive(true);
        shildSlider.maxValue = m_currentShield;
        shildSlider.value = m_currentLife;

        UpdateShildSlider();
    }

    public override void RemoveShild()
    {
        m_view.RPC("RemoveShildRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RemoveShildRPC()
    {
        shildSlider.gameObject.SetActive(false);
        m_currentShield = 0;
    }

    // HUD UPDATE SHIELD

    public void UpdateShildSlider()
    {
        m_view.RPC("UpdateShildSliderRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void UpdateShildSliderRPC()
    {
        shildSlider.value = m_currentShield;
        shildText.text = m_currentShield + " / " + shildSlider.maxValue;
    }
}
