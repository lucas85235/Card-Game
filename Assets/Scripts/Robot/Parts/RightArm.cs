using UnityEngine;

[CreateAssetMenu(fileName = "RightArm", menuName = "ScriptableObjects/Parts/RightArm", order = 5)]
public class RightArm : RobotPart
{
    [Header("Special Sprite")]
    [SerializeField] private Sprite armSprite;
    [SerializeField] private Sprite handSprite;

    public override void SetRobotPart(RobotData robotData, RobotPartItem robotPartItem)
    {
        robotData.SetRightArm(this);

        if (robotPartItem.assignIndex != -1)
        {
            robotData.SetRightArm(null);
        }
    }

    public Sprite ArmSprite() => armSprite;
    public Sprite HandSprite() => handSprite;
}
