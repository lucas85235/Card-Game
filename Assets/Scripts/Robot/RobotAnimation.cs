using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimation : MonoBehaviour
{
    [Header("Torso")]
    [SerializeField] private SpriteRenderer headSpriteRen;
    [SerializeField] private SpriteRenderer neckSpriteRen;
    [SerializeField] private SpriteRenderer torsoSpriteRen;
    [SerializeField] private SpriteRenderer waistSpriteRen;

    [Header("Right Arms")]
    [SerializeField] private SpriteRenderer rightShoulderSpriteRen;
    [SerializeField] private SpriteRenderer rightArmSpriteRen;
    [SerializeField] private SpriteRenderer rightHandSpriteRen;

    [Header("Left Arms")]
    [SerializeField] private SpriteRenderer leftShoulderSpriteRen;
    [SerializeField] private SpriteRenderer leftArmSpriteRen;
    [SerializeField] private SpriteRenderer leftHandSpriteRen;

    [Header("Legs")]
    [SerializeField] private SpriteRenderer bowlSpriteRen;
    [SerializeField] private SpriteRenderer rightLegSpriteRen;
    [SerializeField] private SpriteRenderer rightFootSpriteRen;
    [SerializeField] private SpriteRenderer leftLegSpriteRen;
    [SerializeField] private SpriteRenderer leftFootSpriteRen;

    private Animator m_Animator;

    private void Awake()
    {
        TryGetComponent(out m_Animator);
        m_Animator.SetBool("ResetToIdle", true);
    }

    public void PlayAnimation(Animations newAnimation) => m_Animator.SetTrigger(newAnimation.ToString());
    public void ResetToIdleAfterAnimation(bool value) => m_Animator.SetBool("ResetToIdle", value);
    
    public void ChangeRobotSprites(RobotData newRobot)
    {
        var newSprites = newRobot.Sprites();

        headSpriteRen.sprite = newSprites["head"];
        neckSpriteRen.sprite = newSprites["head-special"];
        torsoSpriteRen.sprite = newSprites["torso"];
        waistSpriteRen.sprite = newSprites["torso-special"];

        bowlSpriteRen.sprite = newSprites["leg-bowl"];
        rightLegSpriteRen.sprite = newSprites["leg-right"];
        rightFootSpriteRen.sprite = newSprites["hoot-right"];
        leftLegSpriteRen.sprite = newSprites["leg-left"];
        leftFootSpriteRen.sprite = newSprites["foot-left"];

        rightShoulderSpriteRen.sprite = newSprites["right-arm-shouder"];
        rightArmSpriteRen.sprite = newSprites["right-arm"];
        if ( newSprites.ContainsKey("right-arm-hand") )
            rightHandSpriteRen.sprite = newSprites["right-arm-hand"];

        leftShoulderSpriteRen.sprite = newSprites["left-arm-shouder"];
        leftArmSpriteRen.sprite = newSprites["left-arm"];
        if ( newSprites.ContainsKey("left-arm-hand") )
            leftHandSpriteRen.sprite = newSprites["left-arm-hand"];
    }
}
