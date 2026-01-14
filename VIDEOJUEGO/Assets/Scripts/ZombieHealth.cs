using System;
using UnityEngine;
using UnityEngine.AI;

public class ZombieHealth : MonoBehaviour
{
    public static event Action<ZombieHealth> OnAnyZombieDied;

    [Header("Health")]
    public float maxHealth = 100f;

    [Header("Points")]
    public int pointsOnKill = 10;


    [SerializeField] private float health;
    private bool dead;

    void Awake()
    {
        ResetHealthToMax();
        dead = false;
    }

    public void TakeDamage(float amount)
    {
        if (dead) return;
        if (amount <= 0f) return;

        health -= amount;
        if (health <= 0f)
        {
            health = 0f;
            Die();
        }
    }

    private void Die()
    {
        if (dead) return;
        dead = true;

        if (PointsManager.Instance != null)
            PointsManager.Instance.AddPoints(pointsOnKill);


        OnAnyZombieDied?.Invoke(this);

        // 1) Animación de muerte
        Animator anim = GetComponentInChildren<Animator>(true);
        if (anim != null)
        {
            anim.SetInteger("State", 3);
        }

        // 2) Desactivar TODOS los scripts del root excepto ZombieHealth
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (var s in scripts)
        {
            if (s == null) continue;
            if (ReferenceEquals(s, this)) continue;
            s.enabled = false;
        }

        // 3) Parar NavMeshAgent sin warnings
        if (TryGetComponent(out NavMeshAgent agent))
        {
            if (agent.isOnNavMesh)
                agent.isStopped = true;

            agent.enabled = false;
        }

        // 4) Desactivar colliders para que no bloquee ni reciba hits
        Collider[] cols = GetComponentsInChildren<Collider>(true);
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i] != null) cols[i].enabled = false;
        }

        // 5) Destruir después (deja tiempo a la animación)
        Destroy(gameObject, 5f);
    }

    public bool IsDead() => dead;

    public void ResetHealthToMax()
    {
        health = maxHealth;
    }

    public float GetHealth() => health;
}
