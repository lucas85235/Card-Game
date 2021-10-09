using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBridge : MonoBehaviour
{
    private Energy m_playerEnergy;    

    void Start()
    {
        m_playerEnergy = GameObject.FindGameObjectWithTag("Player").GetComponent<Energy>();
    }

    public void EndRound()
    {
        m_playerEnergy.EndRound();
    }
}
