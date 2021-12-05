using UnityEngine;

[CreateAssetMenu(fileName = "SetCardCriChance", menuName = "ScriptableObjects/Skills/SetCritChance")]
public class CardSkill_SetCritChance : CardSkill
{
    [SerializeField] private float newCritChance;
    public override void ApplySkill(CardData card)
    {
        base.ApplySkill(card);
        card.CriticalChance = newCritChance;
    }
}

