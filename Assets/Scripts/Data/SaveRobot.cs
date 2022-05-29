using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveRobotDeck", menuName = "ScriptableObjects/SaveRobot", order = 1)]
public class SaveRobot : ScriptableObject
{
    public List<string> SaveCodes = new List<string>(5);
}
