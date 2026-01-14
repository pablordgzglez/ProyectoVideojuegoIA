using UnityEngine;
using UnityEngine.AI;

public class ZombieStats : MonoBehaviour
{
    [Header("Base (Round 1)")]
    public float baseHealth = 100f;
    public float baseSpeed = 2.5f;
    public int baseDamage = 10;
    public float baseAttackCooldown = 1.0f;

    [Header("Scaling per Round")]
    public float healthPerRound = 25f;
    public float speedPerRound = 0.5f;
    public int damagePerRound = 3;
    public float cooldownReductionPerRound = 0.03f;

    [Header("Caps")]
    public float maxSpeed = 6f;
    public int maxDamage = 25;
    public float minAttackCooldown = 0.45f;

    [Header("Points Scaling")]
    public int basePointsOnKill = 10;
    public int extraPointsPerRound = 2;
    public int maxPointsOnKill = 50;

    [Header("Debug")]
    public bool logAppliedStats = false;

    public void ApplyRound(int round)
    {
        int r = Mathf.Max(1, round);

        // Calcular valores escalados
        float hp = baseHealth + (r - 1) * healthPerRound;

        float spd = baseSpeed + (r - 1) * speedPerRound;
        spd = Mathf.Min(spd, maxSpeed);

        int dmg = baseDamage + (r - 1) * damagePerRound;
        dmg = Mathf.Min(dmg, maxDamage);

        float cd = baseAttackCooldown - (r - 1) * cooldownReductionPerRound;
        cd = Mathf.Max(cd, minAttackCooldown);

        int pts = basePointsOnKill + (r - 1) * extraPointsPerRound;
        pts = Mathf.Min(pts, maxPointsOnKill);

        // Aplicar a componentes
        ZombieHealth zh = GetComponent<ZombieHealth>();
        if (zh != null)
        {
            zh.maxHealth = hp;
            zh.ResetHealthToMax(); // spawnea con vida llena según ronda
            zh.pointsOnKill = pts; // puntos por kill según ronda
        }

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = spd;
        }

        ZombieCombat combat = GetComponent<ZombieCombat>();
        if (combat != null)
        {
            combat.damage = dmg;
            combat.attackCooldown = cd;
        }

        if (logAppliedStats)
        {
            Debug.Log($"[ZombieStats] Round {r} -> HP:{hp} SPD:{spd} DMG:{dmg} CD:{cd} PTS:{pts}", gameObject);
        }
    }
}
