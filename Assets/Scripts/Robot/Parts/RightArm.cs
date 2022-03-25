using UnityEngine;

[CreateAssetMenu(fileName = "RightArm", menuName = "ScriptableObjects/Parts/RightArm", order = 5)]
public class RightArm : RobotPart
{
    [Header("Special Sprite")]
    [SerializeField] private Sprite armSprite;
    [SerializeField] private Sprite handSprite;

    public override void SetRobotPart(PlayerData playerData, string partCode)
    {
        playerData.Robot.SetRightArm(this);
        playerData.ConnectedPartCodes[2] = partCode;
    }

    public Sprite ArmSprite() => armSprite;
    public Sprite HandSprite() => handSprite;

    public override void AddPartToPlayer(PlayerData playerData, string partCode)
    {
        playerData.PartsDict[2].Add(partCode);
    }
}
