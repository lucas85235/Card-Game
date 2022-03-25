using UnityEngine;

[CreateAssetMenu(fileName = "Leg", menuName = "ScriptableObjects/Parts/Leg", order = 6)]
public class Leg : RobotPart
{
    [Header("Special Sprite")]
    [SerializeField] private Sprite rightLeg;
    [SerializeField] private Sprite leftLeg;
    [SerializeField] private Sprite rightFoot;
    [SerializeField] private Sprite leftFoot;

    public override void SetRobotPart(PlayerData playerData, string partCode)
    {
        playerData.Robot.SetLeg(this);
        playerData.ConnectedPartCodes[4] = partCode;
    }

    public Sprite RightLeg() => rightLeg;
    public Sprite LeftLeg() => leftLeg;
    public Sprite RightFoot() => rightFoot;
    public Sprite LeftFoot() => leftFoot;

    public override void AddPartToPlayer(PlayerData playerData, string partCode)
    {
        playerData.PartsDict[4].Add(partCode);
    }
}