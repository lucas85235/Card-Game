using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public List<GameObject> energyBars;

    public bool showOnStart = false;

    public int EnergyAmount { get => currentAmount; }
    private int currentAmount;
    private int maxAmount;
    private int initialAmount;

    public int EnergyRoundAmount { get => currentRoundAmount; }
    private int currentRoundAmount;

    private void Start()
    {
        if (gameObject.tag == "Cpu")
        {
            showOnStart = true;
        }

        InitGame();

        EnergyCharge();
        
        Round.i.StartTurn.AddListener(() => EnergyText(currentRoundAmount));
        Round.i.EndTurn.AddListener(EnergyCharge);
    }

    public void InitGame() 
    {
        currentAmount = 5;
        maxAmount = 5;
        initialAmount = 5;
        EnergyText(currentAmount);
    }

    public void UseRoundEnergy(int value)
    {
        currentRoundAmount += value;
        
        if (currentRoundAmount > currentAmount)
        {
            Debug.LogWarning("Energy Have Problem");
        }

        if (!showOnStart) EnergyText(currentRoundAmount);
    }

    private void EnergyCharge() 
    {
        currentAmount = maxAmount;
        
        currentRoundAmount = currentAmount;
        EnergyText(currentAmount);
    }

    public void ChangeMaxEnergyAmount(int setValue)
    {
        maxAmount = initialAmount + setValue;

        currentAmount = maxAmount;
        currentRoundAmount = maxAmount;
        EnergyText(currentAmount);
    }

    private void EnergyText(int value)
    {
        for (int i = 0; i < energyBars.Count; i++)
        {
            energyBars[i].SetActive(value > i);
        }
    }
}
