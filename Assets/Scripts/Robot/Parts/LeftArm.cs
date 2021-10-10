using UnityEngine;

[CreateAssetMenu(fileName = "LeftArm", menuName = "ScriptableObjects/Parts/LeftArm", order = 4)]
public class LeftArm : RobotPart
{
    [Header("SpecialSprite")]
    [SerializeField] private Sprite shoulderSprite;

    public Sprite SpecialSprite() { return shoulderSprite; }
}
