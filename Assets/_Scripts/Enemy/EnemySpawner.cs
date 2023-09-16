using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnRadius;
    [SerializeField] private float spawnRateInSeconds;
    [SerializeField] private EnemyController enemyToSpawn;

    private List<EnemyController> enemyInstances = new List<EnemyController>();
    private bool isSpawning = false;

    void Start() {
        EnemyController.OnEnemyDeath += HandleEnemyDeath;
    }

    public void ToggleSpawning()
    {
        if (isSpawning) {
            CancelInvoke(nameof(SpawnEnemy));
            isSpawning = false;

            enemyInstances.Clear();
        } else {
            InvokeRepeating(nameof(SpawnEnemy), spawnRateInSeconds, spawnRateInSeconds);
            isSpawning = true;
        }
    }

    private void SpawnEnemy() {
        Vector3 randomPoint = GetRandomPointInCircle(
            this.transform.position + new Vector3(0,10f,0),
            spawnRadius
            );

        RaycastHit hit;
        if (Physics.Raycast(randomPoint, Vector3.down, out hit, 999f)) {
            EnemyController enemy = Instantiate<EnemyController>(enemyToSpawn, hit.point, Quaternion.identity);
            enemyInstances.Add(enemy);
        }
    }

    private void HandleEnemyDeath(EnemyController controller)
    {
        if(enemyInstances.Contains(controller)) {
            enemyInstances.Remove(controller);
        }
    }

    Vector3 GetRandomPointInCircle(Vector3 center, float radius) {
        float angle = Random.Range(0, 2f * Mathf.PI);
        return center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * Random.Range(0, radius);
    }
}
