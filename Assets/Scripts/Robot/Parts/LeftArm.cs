using UnityEngine;

[CreateAssetMenu(fileName = "LeftArm", menuName = "ScriptableObjects/Parts/LeftArm", order = 4)]
public class LeftArm : RobotPart
{
    [Header("Special Sprite")]
    [SerializeField] private Sprite armSprite;
    [SerializeField] private Sprite handSprite;


    public override void SetRobotPart(RobotData robotData, RobotPartItem robotPartItem)
    {
        robotData.SetLeftArm(this);

        if (robotPartItem.assignIndex != -1)
        {
            robotData.SetLeftArm(null);
        }
    }

    public Sprite ArmSprite() => armSprite;
    public Sprite HandSprite() => handSprite;
}
