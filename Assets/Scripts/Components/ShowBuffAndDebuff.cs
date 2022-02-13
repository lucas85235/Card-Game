using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowBuffAndDebuff : MonoBehaviour
{
    [Header("Setup")]
    public DeckOf deckOf;
    public TextMeshProUGUI attack;
    public TextMeshProUGUI defense;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI inteligence;
    public TextMeshProUGUI fireResistence;
    public TextMeshProUGUI electricResistence;
    public TextMeshProUGUI waterResistence;
    public TextMeshProUGUI acidResistence;

    private Robot m_robot;

    private IEnumerator Start()
    {
        if (GameController.i.isMultiplayer)
        {
            yield return new WaitUntil( () => BasicConection.Instance.IsReady());
        }
        
        // if (deckOf == DeckOf.player)
        // {
        //     m_robot = GameObject.FindGameObjectWithTag("Player").GetComponent<Robot>();
        //     SetStatsLeft();
        // }
        // else
        // {
        //     m_robot = GameObject.FindGameObjectWithTag("Cpu").GetComponent<Robot>();
        //     SetStatsRight();
        // }
    }

    private void FixedUpdate()
    {
        // if (GameController.i.isMultiplayer && !BasicConection.Instance.InRoon) return;

        // if (deckOf == DeckOf.player)
        // {
        //     SetStatsLeft();
        // }
        // else SetStatsRight();
    }

    public void SetStatsLeft()
    {
        attack.text = "ATK: " + GetStatusWithColor(Stats.attack);
        defense.text = "DEF: " + GetStatusWithColor(Stats.defence);
        speed.text = "SPE: " + GetStatusWithColor(Stats.speed);
        inteligence.text = "INT: " + GetStatusWithColor(Stats.inteligence);
        fireResistence.text = "FRE: " + GetStatusWithColor(Stats.fireResistence);
        electricResistence.text = "ERE: " + GetStatusWithColor(Stats.electricResistence);
        waterResistence.text = "WRE: " + GetStatusWithColor(Stats.waterResistence);
        acidResistence.text = "ARE: " + GetStatusWithColor(Stats.acidResistence);
    }
    public void SetStatsRight()
    {
        attack.text = GetStatusWithColor(Stats.attack) + " :ATK";
        defense.text = GetStatusWithColor(Stats.defence) + " :DEF";
        speed.text = GetStatusWithColor(Stats.speed) + " :SPE";
        inteligence.text = GetStatusWithColor(Stats.inteligence) + " :INT";
        fireResistence.text = GetStatusWithColor(Stats.fireResistence) + " :FRE";
        electricResistence.text = GetStatusWithColor(Stats.electricResistence) + " :ERE";
        waterResistence.text = GetStatusWithColor(Stats.waterResistence) + " :WRE";
        acidResistence.text = GetStatusWithColor(Stats.acidResistence) + " :ARE";
    }

    // Blue 788CFF
    // Green 99F050
    // Red FD4902

    private string GetStatusWithColor(Stats statTyper)
    {        
        int diff = m_robot.StatDiff(statTyper);
        int currentStat = m_robot.CurrentRobotStats[statTyper];

        if (diff != 0)
        {
            if (diff < 0)
            {
                return "<color=#FD4902>" + currentStat + "</color>";
            }
            else return "<color=#99F050>" + currentStat + "</color>";
        }

        return "<color=#ffffff>" + currentStat + "</color>";
    }
}
