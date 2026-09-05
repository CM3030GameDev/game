using UnityEngine;

public class TutorialMultiEnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnInfo
    {
        public MobManager.EnemyTypes enemyType;
        public Transform spawnPoint;
    }

    [Header("Enemies To Spawn")]
    [SerializeField] private EnemySpawnInfo[] enemies;

    [Header("Settings")]
    [SerializeField] private bool spawnOnlyOnce = true;

    private bool hasSpawned = false;

    public void SpawnEnemies()
    {
        if (spawnOnlyOnce && hasSpawned)
        {
            return;
        }

        if (MobManager.Instance == null)
        {
            Debug.LogError(
                "TutorialMultiEnemySpawner: MobManager not found."
            );

            return;
        }

        foreach (EnemySpawnInfo enemyInfo in enemies)
        {
            if (enemyInfo.spawnPoint == null)
            {
                Debug.LogWarning(
                    "A tutorial enemy spawn point has not been assigned."
                );

                continue;
            }

            MobManager.Instance.SpawnEnemyAtPosition(
                enemyInfo.enemyType,
                enemyInfo.spawnPoint.position
            );
        }

        hasSpawned = true;

        Debug.Log(
            "Fire Mode tutorial enemies spawned."
        );
    }
}