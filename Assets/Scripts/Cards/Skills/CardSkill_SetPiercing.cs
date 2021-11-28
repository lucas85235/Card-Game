using UnityEngine;

[CreateAssetMenu(fileName = "SetCardPiercing", menuName = "ScriptableObjects/Skills/SetPiercing")]
public class CardSkill_SetPiercing : CardSkill
{
    public override void ApplySkill(CardData card)
    {
        base.ApplySkill(card);
        card.Piercing = true;
    }
}
