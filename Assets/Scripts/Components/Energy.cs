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
        GameController.i.OnStartTurn.AddListener(() => EnergyCharge());
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
        GameController.i.EndCountdown();
        OnEndRound?.Invoke();
    }

    private void EnergyCharge() 
    {
        Debug.Log("EnergyCharge");
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
        else Debug.LogWarning("energyText of " + name + " is null");
    }

    private void OnDestroy()
    {
        OnEndRound = null;
    }
}
