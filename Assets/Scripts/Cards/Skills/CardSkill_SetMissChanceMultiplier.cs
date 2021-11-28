using UnityEngine;

[CreateAssetMenu(fileName = "SetCardMissChanceMultiplier", menuName = "ScriptableObjects/Skills/SetMissChanceMultiplier")]
public class CardSkill_SetMissChanceMultiplier : CardSkill
{
    [SerializeField] private float newMissChanceMultiplier;
    public override void ApplySkill(CardData card)
    {
        base.ApplySkill(card);
        card.MissChanceMultiplier = newMissChanceMultiplier;
    }
}
