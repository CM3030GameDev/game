using UnityEngine;

public class TutorialCombatSpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField]
    private MobManager.EnemyTypes enemyType =
        MobManager.EnemyTypes.BLUEMOB;

    [SerializeField] private Transform spawnPoint;

    [Header("Settings")]
    [SerializeField] private bool spawnOnlyOnce = true;

    private bool hasSpawned = false;

    // This function can be called by a tutorial trigger.
    public void SpawnTutorialEnemy()
    {
        if (spawnOnlyOnce && hasSpawned)
        {
            return;
        }

        if (MobManager.Instance == null)
        {
            Debug.LogError(
                "TutorialCombatSpawner: MobManager could not be found."
            );

            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError(
                "TutorialCombatSpawner: Spawn Point has not been assigned."
            );

            return;
        }

        GameObject enemy =
            MobManager.Instance.SpawnEnemyAtPosition(
                enemyType,
                spawnPoint.position
            );

        if (enemy != null)
        {
            hasSpawned = true;

            Debug.Log(
                "Tutorial enemy spawned: " + enemyType
            );
        }
    }
}