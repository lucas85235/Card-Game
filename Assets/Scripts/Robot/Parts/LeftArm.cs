using UnityEngine;

[CreateAssetMenu(fileName = "LeftArm", menuName = "ScriptableObjects/Parts/LeftArm", order = 4)]
public class LeftArm : RobotPart
{
    [Header("SpecialSprite")]
    [SerializeField] private Sprite shoulderSprite;


    public override void SetRobotPart(RobotData robotData, RobotPartItem robotPartItem)
    {
        robotData.SetLeftArm(this);

        if (robotPartItem.assignIndex != -1)
        {
            robotData.SetLeftArm(null);
        }
    }

    public Sprite SpecialSprite() { return shoulderSprite; }
}
