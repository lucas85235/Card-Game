using UnityEngine;

[CreateAssetMenu(fileName = "RightArm", menuName = "ScriptableObjects/Parts/RightArm", order = 5)]
public class RightArm : RobotPart
{
    [Header("SpecialSprite")]
    [SerializeField] private Sprite shoulderSprite;

    public override void SetRobotPart(RobotData robotData, RobotPartItem robotPartItem)
    {
        robotData.SetRightArm(this);

        if (robotPartItem.assignIndex != -1)
        {
            robotData.SetRightArm(null);
        }
    }

    public Sprite SpecialSprite() { return shoulderSprite; }
}
