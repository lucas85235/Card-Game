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
    private int maxAmount;
    private int initialAmount;

    public int EnergyRoundAmount { get => currentRoundAmount; }
    private int currentRoundAmount;

    private void Awake()
    {
        if (gameObject.tag == "Player")
        {
            energyText = GameObject.FindGameObjectWithTag("Energy").GetComponent<Text>();
        }        
    }

    private void Start()
    {
        EnergyCharge();
        Round.i.EndTurn.AddListener(() => EnergyCharge());
    }

    public void InitGame() 
    {
        currentAmount = 5;
        maxAmount = 5;
        initialAmount = 5;
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

    private void EnergyCharge() 
    {
        Debug.Log("EnergyCharge");
        
        currentAmount = maxAmount;
        
        currentRoundAmount = currentAmount;
        EnergyText(currentAmount);
    }

    public void ChangeMaxEnergyAmount(int setValue)
    {
        maxAmount = initialAmount + setValue;
    }

    private void EnergyText(int value) 
    {
        if (energyText != null) 
        {
            energyText.text = value + " / " + maxAmount;
        }
        else Debug.LogWarning("energyText of " + name + " is null");
    }
}
