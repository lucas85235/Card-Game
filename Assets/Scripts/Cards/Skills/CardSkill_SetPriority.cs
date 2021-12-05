using UnityEngine;

[CreateAssetMenu(fileName = "SetCardPriority", menuName = "ScriptableObjects/Skills/SetPriority")]
public class CardSkill_SetPriority : CardSkill
{
    [SerializeField] [Range(0,4)] private int newPriorityLevel;

    public override void ApplySkill(CardData card)
    {
        base.ApplySkill(card);
        card.Priority = newPriorityLevel;
    }
}
