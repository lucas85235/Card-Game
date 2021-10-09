using UnityEngine;

[CreateAssetMenu(fileName = "ReduceStats", menuName = "ScriptableObjects/Effects/ReduceStats")]
public class Effect_ReduceStats : Effect
{
    [SerializeField] private Stats statToModify;

    protected override bool ApplyEffect(GameObject emitter, int value, float applicationChance)
    {
        if(!base.ApplyEffect(emitter, value, applicationChance)) return false;

        return true;
    }
}
