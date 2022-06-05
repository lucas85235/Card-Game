using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    public PlayerData PlayerInfo { get; private set; } = new PlayerData();

    public ItemsDB ItemsDB { get; private set; }

    public List<string> saveCodes = new List<string>(5);
    public List<string> SaveCodes
    {
        get => saveCodes;
        set
        {
            saveCodes = value;
        }
    }

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

        LoadData();
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

        // Save Code Handle

        if (SaveCodes.Count == 0)
            SaveCodes = new List<string>(5) { "", "", "", "", "" };

        for (int i = 0; i < SaveCodes.Count; i++)
        {
            if (!PlayerPrefs.HasKey("part" + i))
            {
                PlayerPrefs.SetString("part" + i, "code" + (i + 1));
                SaveCodes[i] = PlayerPrefs.GetString("part" + i);
            }
            else SaveCodes[i] = PlayerPrefs.GetString("part" + i);
        }

        PlayerPrefs.Save();

        foreach (var code in SaveCodes)
        {
            DataManager.Instance.AssignPartToRobot(code);
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
        if (SaveCodes.Count == 0)
            SaveCodes = new List<string>(5) { "", "", "", "", "" };

        SaveCodes[index] = code;

        PlayerPrefs.SetString("part" + index.ToString(), code);
        PlayerPrefs.Save();
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

    private void OnDestroy()
    {
        if (Instance == this)
        {
            PlayerPrefs.DeleteKey("Open-Game");
        }
    }
}
