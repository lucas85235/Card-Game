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
        attack.text = "ATK: " + SetDiff(m_robot.Data().Attack(), m_robot.AttackDiff());
        defense.text = "DEF: " + SetDiff(m_robot.Data().Defense(), m_robot.DefenseDiff());
        speed.text = "SPE: " + SetDiff(m_robot.Data().Speed(), m_robot.SpeedDiff());
    }

    // azul 788CFF
    // verde 99F050
    // vermelho FD4902

    private string SetDiff(int value, int diff)
    {
        if (diff != 0)
        {
            int total = value - diff;

            if (total < value)
            {
                return "<color=#FD4902>" + total + "</color>";
            }
            else return "<color=#99F050>" + total + "</color>";
        }

        return "<color=#ffffff>" + value + "</color>";
    }

    public void SetStatsRight()
    {
        attack.text = SetDiff(m_robot.Data().Attack(), m_robot.AttackDiff()) + " :ATK";
        defense.text = SetDiff(m_robot.Data().Defense(), m_robot.DefenseDiff()) + " :ATK";
        speed.text = SetDiff(m_robot.Data().Speed(), m_robot.SpeedDiff()) + " :ATK";
    }
}
