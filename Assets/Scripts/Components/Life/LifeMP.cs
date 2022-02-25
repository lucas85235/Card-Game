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

    // HUD UPDATE

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
}
