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
            var damage = emitter.Attack() + value - target.Defense();
         
            // Para não curar o adversario
            damage = damage < 1 ? 0 : damage; 

            // Debug.Log("ATTACK: " + emitter.Attack());
            // Debug.Log("RAND: " + value);
            // Debug.Log("DEF: " + emitter.Defense());
            // Debug.Log("DAMAGE: " + damage);

            target.life.TakeDamage(damage);
        }

        return true;
    }
}
