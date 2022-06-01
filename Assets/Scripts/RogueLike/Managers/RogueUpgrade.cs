using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueUpgrade : MonoBehaviour
{
    private Dictionary<string, int> upgrades;
    
    private void Start()
    {
        upgrades = new Dictionary<string, int>();

        upgrades.Add("attack", 0);
        upgrades.Add("defense", 0);
        upgrades.Add("intelligence", 0);
        upgrades.Add("velocity", 0);

        foreach (var key in upgrades.Keys)
        {
            if (PlayerPrefs.HasKey(key))
            {
                upgrades[key] = PlayerPrefs.GetInt(key);
            }
            else PlayerPrefs.SetInt(key, upgrades[key]);
        }
    }
}
