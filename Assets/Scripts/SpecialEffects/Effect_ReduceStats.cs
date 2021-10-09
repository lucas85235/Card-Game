using UnityEngine;

[CreateAssetMenu(fileName = "ReduceStats", menuName = "ScriptableObjects/Effects/ReduceStats")]
public class Effect_ReduceStats : Effect
{
    [SerializeField] private Stats statToModify;

    protected override bool ApplyEffect(Robot emitter, Robot target, int value, float applicationChance)
    {
        if(!base.ApplyEffect(emitter, target, value, applicationChance)) return false;

        return true;
    }
}
