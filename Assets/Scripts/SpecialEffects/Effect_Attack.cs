using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Effects/Attack")]
public class Effect_Attack : Effect
{
    [SerializeField] private Attacks attack;

    protected override bool ApplyEffect(Robot emitter, Robot target, int value, float applicationChance)
    {
        if (!base.ApplyEffect(emitter, target, value, applicationChance)) return false;

        if (target != null)
        {
            target.life.TakeDamage(value);
        }

        return true;
    }
}
