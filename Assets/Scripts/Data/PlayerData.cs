using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData : MonoBehaviour
{
    public RobotData[] Robots { get; set; }
    public Dictionary<string, RobotPartItem> PartsInventory { get; set; }

    public int CurrentRobotIndex { get; set; }

    public void PrepareClass()
    {
        Robots = new RobotData[4];
        
        for (int i = 0; i < Robots.Length; i++)
        {
            Robots[i] = new RobotData();
        }

        PartsInventory = new Dictionary<string, RobotPartItem>();
    }
}