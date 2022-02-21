using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LifeMP : MonoBehaviourPunCallbacks
{
    [Header("Life Settings")]
    public GameObject win;
    public GameObject lose;

    [Header("Life Settings")]
    public Slider lifeSlider;
    public TextMeshProUGUI lifeText;

    [Header("Debug")]
    private int m_currentLife;
    private int m_maxLife;
    public int CurrentLife { get => m_currentLife; }

    private RobotAnimation m_RobotAnimation;
    private PhotonView m_view;

    private void Start()
    {
        TryGetComponent(out m_RobotAnimation);
        TryGetComponent(out m_view);

        int rand = Random.Range(12, 30);
        LifeSetup(rand);
    }

    private void Update()
    {
        if (!m_view.IsMine) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakeDamage(4);
        }
    }

    public override void OnJoinedRoom()
    {
        UpdateLifeSlider();
    }

    public void LifeSetup(int life)
    {
        m_view.RPC("LifeSetupRPC", RpcTarget.AllViaServer, life);
    }

    [PunRPC]
    private void LifeSetupRPC(int life)
    {
        m_maxLife = life;
        lifeSlider.maxValue = m_maxLife;
        lifeSlider.value = m_maxLife;
        m_currentLife = (int) lifeSlider.value;

        UpdateLifeSliderRPC();
    }

    public void TakeDamage(int damage = 1)
    {
        m_view.RPC("TakeDamageRPC", RpcTarget.AllBuffered, damage);
    }

    [PunRPC]
    private void TakeDamageRPC(int damage = 1)
    {
        m_currentLife -= damage;

        if (m_currentLife <= 0)
        {
            Debug.Log(m_view.Owner.NickName + " Is Dead!");
            m_view.RPC("GameOver", RpcTarget.AllViaServer);
        }

        UpdateLifeSlider();
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

    private void UpdateLifeSlider()
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
