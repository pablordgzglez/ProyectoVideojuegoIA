using UnityEngine;

public class ZombieAttackEventReceiver : MonoBehaviour
{
    public void MakeAttack()
    {
        var combat = GetComponentInParent<ZombieCombat>();
        if (combat != null) combat.DoAttackDamage();
    }

    public void Z_attack()
    {
        MakeAttack();
    }
}
