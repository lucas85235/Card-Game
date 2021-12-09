using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowBuffAndDebuff : MonoBehaviour
{
    [Header("Set")]
    public DeckOf deckOf;
    public TextMeshProUGUI attack;
    public TextMeshProUGUI defense;
    public TextMeshProUGUI speed;

    private Robot m_robot;

    void Start()
    {
        if (deckOf == DeckOf.player)
        {
            m_robot = GameObject.FindGameObjectWithTag("Player").GetComponent<Robot>();
            SetStatsLeft();
        }
        else
        {
            m_robot = GameObject.FindGameObjectWithTag("Cpu").GetComponent<Robot>();
            SetStatsRight();
        }
    }

    private void FixedUpdate()
    {
        if (deckOf == DeckOf.player)
        {
            SetStatsLeft();
        }
        else
        {
            SetStatsRight();
        }
    }

    public void SetStatsLeft()
    {
        attack.text = "ATK: " + "<color=#ffffff>" + SetDiff(m_robot.Data().Attack(), m_robot.AttackDiff()) + "</color>";
        defense.text = "DEF: " + "<color=#ffffff>" + SetDiff(m_robot.Data().Defense(), m_robot.DefenseDiff()) + "</color>";
        speed.text = "SPE: " + "<color=#ffffff>" + SetDiff(m_robot.Data().Speed(), m_robot.SpeedDiff()) + "</color>";
    }

    // azul 788CFF
    // verde 99F050
    // vermelho FD4902

    private int SetDiff(int value, int diff)
    {
        // Debug.Log (value + " - " +  diff);
        if (diff != 0)
        {
            int total = value + (value - diff);
            Debug.Log ("total: " + total);

            return total;
        }

        Debug.Log ("value: " + value);
        return value;
    }

    private void SetNumberColor()
    {

    }

    public void SetStatsRight()
    {
        attack.text = "<color=#ffffff>" + SetDiff(m_robot.Data().Attack(), m_robot.AttackDiff()) + "</color>" + " :ATK";
        defense.text = "<color=#ffffff>" + SetDiff(m_robot.Data().Defense(), m_robot.DefenseDiff()) + "</color>" + " :ATK";
        speed.text = "<color=#ffffff>" + SetDiff(m_robot.Data().Speed(), m_robot.SpeedDiff()) + "</color>" + " :ATK";
    }
}
