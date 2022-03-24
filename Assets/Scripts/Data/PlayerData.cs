using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData : MonoBehaviour
{
    public RobotData Robot { get; set; }

    public Dictionary<string, RobotPartItem> PartsInventory { get; set; }

    /* Index of the connections
     * 0 - head
     * 1 - torso
     * 2 - right arm
     * 3 - left arm
     * 4 - legs
     */
    public Dictionary<int, List<string>> PartsDict { get; set; }
    public string[] ConnectedPartCodes { get; set; }


    public void PrepareClass()
    {
        Robot = new RobotData();

        PartsInventory = new Dictionary<string, RobotPartItem>();
        PartsDict = new Dictionary<int, List<string>>();

        PartsDict[0] = new List<string>();
        PartsDict[1] = new List<string>();
        PartsDict[2] = new List<string>();
        PartsDict[3] = new List<string>();
        PartsDict[4] = new List<string>();

        ConnectedPartCodes = new string[5];
    }

    internal void SetNewConnection(int connectionIndex)
    {
        string oldConnection = ConnectedPartCodes[connectionIndex];

        int partIndex = PartsDict[connectionIndex].IndexOf(oldConnection);

        if (partIndex >= PartsDict[connectionIndex].Count - 1)
        {
            partIndex = 0;
        }
        else
        {
            partIndex++;
        }

        ItemsDB.ItemsDict[PartsInventory[PartsDict[connectionIndex][partIndex]].itemID].SetRobotPart(this, PartsDict[connectionIndex][partIndex]);
    }
}