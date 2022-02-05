using UnityEngine;

[CreateAssetMenu(fileName = "Leg", menuName = "ScriptableObjects/Parts/Leg", order = 6)]
public class Leg : RobotPart
{
    [Header("Special Sprite")]
    [SerializeField] private Sprite rightLeg;
    [SerializeField] private Sprite leftLeg;
    [SerializeField] private Sprite rightFoot;
    [SerializeField] private Sprite leftFoot;

    public override void SetRobotPart(RobotData robotData, RobotPartItem robotPartItem)
    {
        robotData.SetLeg(this);

        if (robotPartItem.assignIndex != -1)
        {
            robotData.SetLeg(null);
        }
    }

    public Sprite RightLeg() => rightLeg;
    public Sprite LeftLeg() => leftLeg;
    public Sprite RightFoot() => rightFoot;
    public Sprite LeftFoot() => leftFoot;
}