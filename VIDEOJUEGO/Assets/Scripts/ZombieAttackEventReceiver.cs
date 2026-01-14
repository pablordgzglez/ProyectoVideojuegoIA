using UnityEngine;

public class ZombieAttackEventReceiver : MonoBehaviour
{
    // Este lo pide tu clip: MakeAttack
    public void MakeAttack()
    {
        var combat = GetComponentInParent<ZombieCombat>();
        if (combat != null) combat.DoAttackDamage();
    }

    // Por si algún clip usa este nombre también
    public void Z_attack()
    {
        MakeAttack();
    }
}
