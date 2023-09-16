using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{   
    [SerializeField] private float roundLength = 100f; // This should be in a GameMode class or something

    [SerializeField] private bool isSpawning;
    [SerializeField] private List<EnemySpawner> enemySpawners = new List<EnemySpawner>();

    void Awake() {
        PlayPhaseManager.OnAfterPlayPhaseChanged += HandlePlayPhaseChange;
    }

    void HandlePlayPhaseChange(PlayPhase newPhase) {
        if (newPhase == PlayPhase.Defence && !isSpawning) {
            EnableSpawners();

            Invoke(nameof(RoundComplete), roundLength);
        }
    }

    void EnableSpawners()
    {
        foreach (EnemySpawner spawner in enemySpawners) {
            if (spawner == null) continue;
            spawner.ToggleSpawning();
        }
        isSpawning = true;
    }

    void RoundComplete() {
        if (isSpawning) {
            isSpawning = false;

            foreach (EnemySpawner spawner in enemySpawners) {
               if (spawner == null) continue;
               spawner.ToggleSpawning();
            }
        }

        PlayPhaseManager.Instance.ChangePhase(PlayPhase.Victory);
    }
}
