using UnityEngine;

[CreateAssetMenu(fileName = "Head", menuName = "ScriptableObjects/Parts/Head", order = 1)]
public class Head : RobotPart
{
    [Header("Special Sprite")]
    [SerializeField] private Sprite neckSprite;

    public override void SetRobotPart(PlayerData playerData, string partCode)
    {
        playerData.Robot.SetHead(this);
        playerData.ConnectedPartCodes[0] = partCode;
    }

    public override void AddPartToPlayer(PlayerData playerData, string partCode)
    {
        playerData.PartsDict[0].Add(partCode);
    }

    public Sprite SpecialSprite() => neckSprite;
}
