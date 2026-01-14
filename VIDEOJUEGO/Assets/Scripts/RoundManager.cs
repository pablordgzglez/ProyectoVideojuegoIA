using System.Collections;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public int currentRound = 1;
    public int zombiesRemaining;

    private UIManager ui;
    public ZombieSpawner zombieSpawner;

    [Header("Banners / Timing")]
    public float roundStartBannerTime = 1.2f;
    public float roundCompleteBannerTime = 1.5f;
    public float delayBeforeSpawn = 0.8f;

    private bool transitioning;

    void OnEnable()
    {
        ZombieHealth.OnAnyZombieDied += HandleZombieDied;
    }

    void OnDisable()
    {
        ZombieHealth.OnAnyZombieDied -= HandleZombieDied;
    }

    void Start()
    {
        ui = FindFirstObjectByType<UIManager>();
        StartFirstRound();
    }

    public void StartFirstRound()
    {
        currentRound = 1;
        StartCoroutine(StartRoundRoutine(currentRound));
    }

    public void StartNextRound()
    {
        currentRound++;
        StartCoroutine(StartRoundRoutine(currentRound));
    }

    private IEnumerator StartRoundRoutine(int round)
    {
        transitioning = true;

        if (ui != null)
        {
            ui.UpdateRound(round);
            ui.ShowRoundBanner("RONDA " + round, roundStartBannerTime);
        }

        yield return new WaitForSeconds(delayBeforeSpawn);

        zombiesRemaining = zombieSpawner.SpawnWave(round);

        Debug.Log("Ronda " + round + " empieza. Zombies: " + zombiesRemaining);

        transitioning = false;
    }

    private void HandleZombieDied(ZombieHealth zh)
    {
        OnZombieKilled();
    }

    public void OnZombieKilled()
    {
        if (transitioning) return;

        zombiesRemaining--;
        Debug.Log("Zombie muerto. Quedan: " + zombiesRemaining);

        if (zombiesRemaining <= 0)
        {
            StartCoroutine(RoundCompletedRoutine());
        }
    }

    private IEnumerator RoundCompletedRoutine()
    {
        transitioning = true;

        if (ui != null)
            ui.ShowRoundBanner("RONDA " + currentRound + " COMPLETADA", roundCompleteBannerTime);

        Debug.Log("Ronda completada");

        yield return new WaitForSeconds(roundCompleteBannerTime);

        StartNextRound();
    }
}
