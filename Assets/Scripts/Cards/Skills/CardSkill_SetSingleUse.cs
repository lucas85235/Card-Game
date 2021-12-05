using UnityEngine;

[CreateAssetMenu(fileName = "SetCardSingleUse", menuName = "ScriptableObjects/Skills/SetSingleUse")]
public class CardSkill_SetSingleUse : CardSkill
{
    public override void ApplySkill(CardData card)
    {
        base.ApplySkill(card);
        card.SingleUse = true;
    }
}
