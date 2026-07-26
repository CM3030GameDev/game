using UnityEngine;

public class Temporary : MonoBehaviour
{
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private SceneState sceneState;
    [SerializeField] private EnemySystem enemySystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sceneState.act = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            characterStats.ResetStats();
            sceneState.ResetStates();
            enemySystem.ResetEnemies();
        }
    }
}
