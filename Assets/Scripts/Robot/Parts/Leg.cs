using UnityEngine;

[CreateAssetMenu(fileName = "Leg", menuName = "ScriptableObjects/Parts/Leg", order = 6)]
public class Leg : RobotPart
{
    public override void SetRobotPart(RobotData robotData, RobotPartItem robotPartItem)
    {
        robotData.SetLeg(this);

        if (robotPartItem.assignIndex != -1)
        {
            robotData.SetLeg(null);
        }
    }
}