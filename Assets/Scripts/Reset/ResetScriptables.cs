using UnityEngine;

public class ResetScriptables : MonoBehaviour
{
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private SceneState sceneState;
    [SerializeField] private EnemySystem enemySystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterStats.ResetStats();
        sceneState.ResetStates();
        enemySystem.ResetEnemies();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
