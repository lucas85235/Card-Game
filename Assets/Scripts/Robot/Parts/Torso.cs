using UnityEngine;

[CreateAssetMenu(fileName = "Torso", menuName = "ScriptableObjects/Parts/Torso", order = 2)]
public class Torso : RobotPart
{
    [Header("Special Sprite")]
    [SerializeField] private Sprite waistSprite;

    public override void SetRobotPart(PlayerData playerData, string partCode)
    {
        playerData.Robot.SetTorso(this);
        playerData.ConnectedPartCodes[1] = partCode;
    }

    public Sprite SpecialSprite() => waistSprite;

    public override void AddPartToPlayer(PlayerData playerData, string partCode)
    {
        playerData.PartsDict[1].Add(partCode);
    }
}
