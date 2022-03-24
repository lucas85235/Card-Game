using UnityEngine;

[CreateAssetMenu(fileName = "LeftArm", menuName = "ScriptableObjects/Parts/LeftArm", order = 4)]
public class LeftArm : RobotPart
{
    [Header("Special Sprite")]
    [SerializeField] private Sprite armSprite;
    [SerializeField] private Sprite handSprite;


    public override void SetRobotPart(PlayerData playerData, string partCode)
    {
        playerData.Robot.SetLeftArm(this);
        playerData.ConnectedPartCodes[3] = partCode;
    }

    public Sprite ArmSprite() => armSprite;
    public Sprite HandSprite() => handSprite;

    public override void AddPartToPlayer(PlayerData playerData, string partCode)
    {
        playerData.PartsDict[3].Add(partCode);
    }
}
