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

        ItemsDB.ItemsDict[partItem.itemID].SetRobotPart(PlayerInfo.Robots[robotIndex], partItem);

        partItem.assignIndex = robotIndex;
    }

    public RobotData GetCurrentRobot()
    {
        return PlayerInfo.Robots[PlayerInfo.CurrentRobotIndex];
    }
}
