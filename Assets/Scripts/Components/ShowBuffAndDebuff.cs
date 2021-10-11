using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowBuffAndDebuff : MonoBehaviour
{
    [Header("Set")]
    public DeckOf deckOf;
    public Text attack;
    public Text defense;
    public Text speed;

    private Robot m_robot;

    void Start()
    {
        if (deckOf == DeckOf.player)
        {
            m_robot = GameObject.FindGameObjectWithTag("Player").GetComponent<Robot>();
            SetStatsLeft();
            GameController.i.AfterApplyEffects.AddListener(() => SetStatsLeft());
        }
        else
        {
            m_robot = GameObject.FindGameObjectWithTag("Cpu").GetComponent<Robot>();
            SetStatsRight();
            GameController.i.AfterApplyEffects.AddListener(() => SetStatsRight());
        }
    }

    public void SetStatsLeft()
    {
        attack.text = "ATK: " + m_robot.AttackDiff();
        defense.text = "DEF: " + m_robot.DefenseDiff();
        speed.text = "SPE: " + m_robot.SpeedDiff();
    }

    public void SetStatsRight()
    {
        attack.text = m_robot.AttackDiff() + " :ATK";
        defense.text = m_robot.DefenseDiff() + " :DEF";
        speed.text = m_robot.SpeedDiff() + " :SPE";
    }
}
