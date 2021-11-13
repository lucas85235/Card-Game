using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    public PlayerData PlayerInfo { get; private set; } = new PlayerData();

    public ItemsDB ItemsDB { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ItemsDB = new ItemsDB();
        PlayerInfo.PrepareClass();
    }

    public void AddPartItem(RobotPartItem newItem, string itemCode)
    {
        if (PlayerInfo.PartsInventory.ContainsKey(itemCode))
        {
            return;
        }
        PlayerInfo.PartsInventory[itemCode] = newItem;
    }

    public void AssignPartToRobot(string partCode, int robotIndex)
    {
        if (!PlayerInfo.PartsInventory.ContainsKey(partCode))
        {
            return;
        }

        RobotPartItem partItem = PlayerInfo.PartsInventory[partCode];

        if (ItemsDB.ItemsDict[partItem.itemID].GetType() == typeof(Head))
        {
            PlayerInfo.Robots[robotIndex].SetHead(ItemsDB.ItemsDict[partItem.itemID] as Head);

            if (partItem.assignIndex != -1) 
            {
                PlayerInfo.Robots[partItem.assignIndex].SetHead(null);
            }
        }
        else if (ItemsDB.ItemsDict[partItem.itemID].GetType() == typeof(Torso))
        {
            PlayerInfo.Robots[robotIndex].SetTorso(ItemsDB.ItemsDict[partItem.itemID] as Torso);

            if (partItem.assignIndex != -1)
            {
                PlayerInfo.Robots[partItem.assignIndex].SetTorso(null);
            }
        }
        else if (ItemsDB.ItemsDict[partItem.itemID].GetType() == typeof(RightArm))
        {
            PlayerInfo.Robots[robotIndex].SetRightArm(ItemsDB.ItemsDict[partItem.itemID] as RightArm);

            if (partItem.assignIndex != -1)
            {
                PlayerInfo.Robots[partItem.assignIndex].SetRightArm(null);
            }
        }
        else if (ItemsDB.ItemsDict[partItem.itemID].GetType() == typeof(LeftArm))
        {
            PlayerInfo.Robots[robotIndex].SetLeftArm(ItemsDB.ItemsDict[partItem.itemID] as LeftArm);

            if (partItem.assignIndex != -1)
            {
                PlayerInfo.Robots[partItem.assignIndex].SetLeftArm(null);
            }
        }
        else if (ItemsDB.ItemsDict[partItem.itemID].GetType() == typeof(Leg))
        {
            PlayerInfo.Robots[robotIndex].SetLeg(ItemsDB.ItemsDict[partItem.itemID] as Leg);

            if (partItem.assignIndex != -1)
            {
                PlayerInfo.Robots[partItem.assignIndex].SetLeg(null);
            }
        }

        partItem.assignIndex = robotIndex;
    }

    public RobotData GetCurrentRobot()
    {
        return PlayerInfo.Robots[PlayerInfo.CurrentRobotIndex];
    }
}
