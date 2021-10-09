using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Esse componente lida com uso de energia
// Possui a total de energia e a energia
// usada durante o round

// Cada robo tera uma quantidade de energia
// Mas neste momento todos terão 5

// A energia tem um limite de 5
// A cada turno a energia volta a 5

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
        currentAmount = 5;
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
        currentAmount = 5;
        if (currentAmount > 5) currentAmount = 5;
        currentRoundAmount = currentAmount;
        EnergyText(currentAmount);
    }

    private void EnergyText(int value) 
    {
        if (energyText != null) 
        {
            energyText.text = value + " / 5";
        }
    }

    private void OnDestroy()
    {
        OnEndRound -= AfterEndRound;
    }
}
