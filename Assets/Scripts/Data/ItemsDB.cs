using System.Collections.Generic;
using UnityEngine;

public class ItemsDB
{
    private RobotPart[] m_AvailableItems;

    public static Dictionary<string, RobotPart> ItemsDict { get; private set; }

    public ItemsDB()
    {
        ItemsDict = new Dictionary<string, RobotPart>();
        m_AvailableItems = Resources.LoadAll<RobotPart>("Parts");

        foreach (var item in m_AvailableItems)
        {
            ItemsDict.Add(item.ID(), item);
        }
    }
}
