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

        print(itemCode);

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

    public void ChangePart(int connectionIndex)
    {
        PlayerInfo.SetNewConnection(connectionIndex);
    }

    public RobotData GetCurrentRobot()
    {
        return PlayerInfo.Robot;
    }
}
