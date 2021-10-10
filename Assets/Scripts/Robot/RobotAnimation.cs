using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer headSpriteRen;
    [SerializeField] private SpriteRenderer torsoSpriteRen;
    [SerializeField] private SpriteRenderer legsSpriteRen;
    [SerializeField] private SpriteRenderer rightShoulderSpriteRen;
    [SerializeField] private SpriteRenderer leftShoulderSpriteRen;
    [SerializeField] private SpriteRenderer rightArmSpriteRen;
    [SerializeField] private SpriteRenderer leftArmSpriteRen;

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
        //Retorna sprites na ordem: Tronco - Pernas - Cabeça - Ombro Direito - Braço Direito - Ombro Esquerdo - Braço Esquerdo
        var newSprites = newRobot.Sprites();

        torsoSpriteRen.sprite = newSprites[0];
        legsSpriteRen.sprite = newSprites[1];
        headSpriteRen.sprite = newSprites[2];
        rightShoulderSpriteRen.sprite = newSprites[3];
        rightArmSpriteRen.sprite = newSprites[4];
        leftShoulderSpriteRen.sprite = newSprites[5];
        leftArmSpriteRen.sprite = newSprites[6];
    }
}
