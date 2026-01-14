using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject zombiePrefab;
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Round Settings")]
    public int baseZombiesPerRound = 6;
    public int extraZombiesPerRound = 4;

    [Header("Spawn Rules")]
    public float minDistanceFromPlayer = 6f;

    private Transform player;

    void Start()
    {
        FindPlayer();
    }

    void FindPlayer()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
            player = p.transform;
    }

    public int SpawnWave(int round)
    {
        int amount = baseZombiesPerRound + (round - 1) * extraZombiesPerRound;
        return SpawnAmount(amount, round);
    }

    int SpawnAmount(int amount, int round)
    {
        if (zombiePrefab == null || spawnPoints.Count == 0)
        {
            Debug.LogError("ZombieSpawner: falta prefab o spawnPoints.");
            return 0;
        }

        if (player == null)
            FindPlayer();

        if (player == null)
        {
            Debug.LogError("ZombieSpawner: no se encontró Player con tag 'Player'.");
            return 0;
        }

        // Filtrar puntos válidos
        List<Transform> validPoints = new List<Transform>();
        foreach (Transform sp in spawnPoints)
        {
            if (sp == null) continue;

            if (Vector3.Distance(sp.position, player.position) >= minDistanceFromPlayer)
                validPoints.Add(sp);
        }

        if (validPoints.Count == 0)
        {
            Debug.LogWarning("ZombieSpawner: no hay spawn points válidos.");
            return 0;
        }

        int spawned = 0;

        for (int i = 0; i < amount; i++)
        {
            Transform sp = validPoints[Random.Range(0, validPoints.Count)];
            GameObject z = Instantiate(zombiePrefab, sp.position, sp.rotation);

            // Asegurar NavMesh
            if (z.TryGetComponent(out NavMeshAgent agent))
                agent.Warp(sp.position);

            ZombieCombat combat = z.GetComponent<ZombieCombat>();
            if (combat != null)
                combat.target = player;

            ZombieStats stats = z.GetComponent<ZombieStats>();
            if (stats != null)
                stats.ApplyRound(round);

            spawned++;
        }

        return spawned;
    }
}
