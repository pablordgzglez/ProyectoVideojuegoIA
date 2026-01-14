using UnityEngine;
using UnityEngine.AI;

public class ZombieCombat : MonoBehaviour
{
    [Header("Refs")]
    public Transform target;
    public NavMeshAgent agent;
    public Animator anim;

    [Header("Distances")]
    public float chaseDistance = 30f;
    public float attackDistance = 2.2f;

    [Header("Attack")]
    public int damage = 10;
    public float attackCooldown = 1.0f;

    [Header("Path Update")]
    public float repathRate = 0.2f;
    private float repathTimer;

    [Header("Rotation")]
    public float turnSpeed = 10f;

    [Header("Attack Point (optional)")]
    public Transform attackPoint;

    private float nextAttackTime;
    private ZombieHealth myHealth;

    void Awake()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (anim == null) anim = GetComponentInChildren<Animator>(true);
        myHealth = GetComponent<ZombieHealth>();

        if (agent != null) agent.updateRotation = false;
        if (attackPoint == null) attackPoint = transform;
    }

    void Start()
    {
        if (target == null)
        {
            Player p = FindFirstObjectByType<Player>();
            if (p != null) target = p.transform;
        }
    }

    void Update()
    {
        if (myHealth != null && myHealth.IsDead()) return;
        if (target == null || agent == null || anim == null) return;
        if (!agent.enabled || !agent.isOnNavMesh) return;

        float d = DistanceToTarget();

        if (d <= attackDistance)
        {
            anim.SetInteger("State", 2);
            agent.isStopped = true;

            RotateTowardsTarget();
            TryDealDamage();
        }
        else if (d <= chaseDistance)
        {
            anim.SetInteger("State", 1);
            agent.isStopped = false;

            repathTimer -= Time.deltaTime;
            if (repathTimer <= 0f)
            {
                agent.SetDestination(target.position);
                repathTimer = repathRate;
            }

            RotateTowardsTarget();
        }
        else
        {
            anim.SetInteger("State", 0);
            agent.isStopped = true;
            if (agent.hasPath) agent.ResetPath();
        }
    }

    // Si algún día usas Animation Event, llama a esto en el frame del golpe
    public void DoAttackDamage()
    {
        TryDealDamage();
    }

    private void TryDealDamage()
    {
        if (Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + attackCooldown;

        float d = DistanceToTarget();
        if (d > attackDistance + 0.3f) return;

        Player player = target.GetComponentInParent<Player>();
        if (player == null) player = target.GetComponentInChildren<Player>();

        if (player != null && !player.IsDead())
        {
            // AQUÍ es donde antes se quedaba fijo en 10.
            // Ahora usa el damage del componente (que ZombieStats.ApplyRound actualiza).
            player.TakeDamage(damage);
        }
    }

    private float DistanceToTarget()
    {
        Vector3 from = (attackPoint != null) ? attackPoint.position : transform.position;
        Vector3 to = target.position;

        from.y = 0f;
        to.y = 0f;

        return Vector3.Distance(from, to);
    }

    private void RotateTowardsTarget()
    {
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
    }
}
