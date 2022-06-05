using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeChips : MonoBehaviour
{
    public Robot robot;

    private void Start()
    {
        robot.CurrentRobotStats[Stats.attack] += RogueItems.Instance.Attack;
        robot.CurrentRobotStats[Stats.defence] += RogueItems.Instance.Defense;
        robot.CurrentRobotStats[Stats.inteligence] += RogueItems.Instance.Intelligence;
        robot.CurrentRobotStats[Stats.speed] += RogueItems.Instance.Velocity;
    }
}
