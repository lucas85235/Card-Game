using UnityEngine;

[CreateAssetMenu(fileName = "StatsBonus", menuName = "ScriptableObjects/Effects/StatsBonus")]
public class Effect_StatsBonus : Effect
{
    [SerializeField] private Stats statToModify;

    protected override bool ApplyEffect(GameObject emitter, int value, float applicationChance)
    {
        if (!base.ApplyEffect(emitter, value, applicationChance)) return false;

        return true;
    }
}
