using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Effects/Attack")]
public class Effect_Attack : Effect
{
    [SerializeField] private Attacks attack;

    protected override bool ApplyEffect(GameObject emitter, int value, float applicationChance)
    {
        if (!base.ApplyEffect(emitter, value, applicationChance)) return false;

        return true;
    }
}
