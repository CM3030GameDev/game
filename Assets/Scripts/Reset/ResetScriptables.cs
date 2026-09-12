using UnityEngine;

public class ResetScriptables : MonoBehaviour
{
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private SceneState sceneState;
    [SerializeField] private EnemySystem enemySystem;
    [SerializeField] private LoadoutState loadout;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterStats.ResetStats();
        sceneState.ResetStates();
        enemySystem.ResetEnemies();

        // Without this a new run starts holding the previous run's weapons and stat levels.
        if (loadout != null) loadout.ResetLoadout();
    }
}
