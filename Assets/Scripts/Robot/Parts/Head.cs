using UnityEngine;

[CreateAssetMenu(fileName = "Head", menuName = "ScriptableObjects/Parts/Head", order = 1)]
public class Head : RobotPart
{
    public override void SetRobotPart(RobotData robotData, RobotPartItem robotPartItem)
    {
        robotData.SetHead(this);

        if (robotPartItem.assignIndex != -1)
        {
            robotData.SetHead(null);
        }
    }
}
