using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    public PlayerData PlayerInfo { get; private set; } = new PlayerData();

    public ItemsDB ItemsDB { get; private set; }

    public SaveRobot data;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ItemsDB = new ItemsDB();
        PlayerInfo.PrepareClass();

        #if UNITY_EDITOR
        LoadData();
        #endif
    }

    private void LoadData()
    {
        var newParts = new List<RobotPartItem>();
        int countParts = 0;

        foreach (var item in Enum.GetValues(typeof(PartID)))
        {
            var part = new RobotPartItem() { itemID = item.ToString() };
            newParts.Add(part);

            countParts++;
        }

        for (int i = 0; i < newParts.Count; i++)
        {
            DataManager.Instance.AddPartItem(newParts[i], "code" + (i + 1));
        }

        if (DataManager.Instance.data.SaveCodes.Count > 0)
        {
            for (int i = 0; i < DataManager.Instance.data.SaveCodes.Count; i++)
            {

                DataManager.Instance.AssignPartToRobot(DataManager.Instance.data.SaveCodes[i]);
                DataManager.Instance.SavePart(DataManager.Instance.data.SaveCodes[i], i);
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                DataManager.Instance.AssignPartToRobot("code" + (i + 1));
                DataManager.Instance.SavePart("code" + (i + 1), i);
            }
        }
    }

    public void AddPartItem(RobotPartItem newItem, string itemCode)
    {
        if (PlayerInfo.PartsInventory.ContainsKey(itemCode))
        {
            return;
        }

        PlayerInfo.PartsInventory[itemCode] = newItem;
        ItemsDB.ItemsDict[newItem.itemID].AddPartToPlayer(PlayerInfo, itemCode);
    }

    public void AssignPartToRobot(string partCode)
    {
        if (!PlayerInfo.PartsInventory.ContainsKey(partCode))
        {
            return;
        }

        RobotPartItem partItem = PlayerInfo.PartsInventory[partCode];
        ItemsDB.ItemsDict[partItem.itemID].SetRobotPart(PlayerInfo, partCode);
    }

    public void SavePart(string code, int index)
    {
        if (data.SaveCodes.Count == 0)
            data.SaveCodes = new List<string>(5) { "", "", "", "", "" };

        data.SaveCodes[index] = code;
    }

    public void ChangePart(int connectionIndex)
    {
        PlayerInfo.SetNewConnection(connectionIndex);
    }

    public Sprite GetPartSprite(string partCode)
    {
        if (!PlayerInfo.PartsInventory.ContainsKey(partCode))
        {
            return null;
        }

        RobotPartItem partItem = PlayerInfo.PartsInventory[partCode];
        return ItemsDB.ItemsDict[partItem.itemID].Sprite();
    }

    public RobotData GetCurrentRobot()
    {
        return PlayerInfo.Robot;
    }
}
