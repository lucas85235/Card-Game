using UnityEngine;

[CreateAssetMenu(fileName = "Head", menuName = "ScriptableObjects/Parts/Head", order = 1)]
public class Head : RobotPart
{
    [Header("Special Sprite")]
    [SerializeField] private Sprite neckSprite;

    public override void SetRobotPart(RobotData robotData, RobotPartItem robotPartItem)
    {
        robotData.SetHead(this);

        if (robotPartItem.assignIndex != -1)
        {
            robotData.SetHead(null);
        }
    }

    public Sprite SpecialSprite() => neckSprite;
}
