using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    [Header("Setup")]
    public Text energyText;

    public int EnergyAmount { get => currentAmount; }
    private int currentAmount;

    public int EnergyRoundAmount { get => currentRoundAmount; }
    private int currentRoundAmount;

    public Action OnEndRound;

    private void Start()
    {
        InitGame();
        OnEndRound += AfterEndRound;
    }

    public void InitGame() 
    {
        currentAmount = 3;
        currentRoundAmount = currentAmount;
        EnergyText(currentAmount);
    }

    public void UseRoundEnergy(int value)
    {
        currentRoundAmount += value;
        
        if (currentRoundAmount > currentAmount)
        {
            Debug.Log("Energy Have Problem");
        }

        EnergyText(currentRoundAmount);
    }

    public void EndRound()
    {
        OnEndRound?.Invoke();
    }

    private void AfterEndRound() 
    {
        Debug.Log("AfterEndRound");
        currentAmount = currentRoundAmount + 2;
        if (currentAmount > 10) currentAmount = 10;
        currentRoundAmount = currentAmount;
        EnergyText(currentAmount);
    }

    private void EnergyText(int value) 
    {
        if (energyText != null) 
        {
            energyText.text = value + " / 10";
        }
    }

    private void OnDestroy()
    {
        OnEndRound -= AfterEndRound;
    }
}
