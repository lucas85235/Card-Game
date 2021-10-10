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
    private Animations m_CurrentAnimation;

    private void Awake()
    {
        TryGetComponent(out m_Animator);
    }

    public void PlayAnimation(Animations newAnimation)
    {
        if (m_CurrentAnimation == newAnimation) return;

        m_CurrentAnimation = newAnimation;
        m_Animator.SetTrigger(newAnimation.ToString());
    }

    #region Sprite Renderers Gets
    public SpriteRenderer HeadSpriteRen () { return headSpriteRen; }
    public SpriteRenderer TorsoSpriteRen() { return torsoSpriteRen; }
    public SpriteRenderer LegsSpriteRen() { return legsSpriteRen; }
    public SpriteRenderer RightShoulderSpriteRen() { return rightShoulderSpriteRen; }
    public SpriteRenderer LeftShoulderSpriteRen() { return leftShoulderSpriteRen; }
    public SpriteRenderer RightArmSpriteRen() { return rightArmSpriteRen; }
    public SpriteRenderer LeftArmSpriteRen() { return leftArmSpriteRen; }
    #endregion
}
