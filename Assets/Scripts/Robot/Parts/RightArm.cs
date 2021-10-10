using UnityEngine;

[CreateAssetMenu(fileName = "RightArm", menuName = "ScriptableObjects/Parts/RightArm", order = 5)]
public class RightArm : RobotPart
{
    [Header("SpecialSprite")]
    [SerializeField] private Sprite shoulderSprite;
    public Sprite SpecialSprite() { return shoulderSprite; }
}
