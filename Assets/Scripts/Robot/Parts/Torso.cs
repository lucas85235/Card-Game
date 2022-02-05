using UnityEngine;

[CreateAssetMenu(fileName = "Torso", menuName = "ScriptableObjects/Parts/Torso", order = 2)]
public class Torso : RobotPart
{
    [Header("Special Sprite")]
    [SerializeField] private Sprite waistSprite;

    public override void SetRobotPart(RobotData robotData, RobotPartItem robotPartItem)
    {
        robotData.SetTorso(this);

        if (robotPartItem.assignIndex != -1)
        {
            robotData.SetTorso(null);
        }
    }

    public Sprite SpecialSprite() => waistSprite;
}
