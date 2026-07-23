using UnityEngine;

public class Temporary : MonoBehaviour
{
    [SerializeField] private CompanionSystem companionSystem;
    [SerializeField] private SkillSystem skillSystem;
    [SerializeField] private SceneState sceneState;
    [SerializeField] private EnemySystem enemySystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        companionSystem.ResetCompanions();
        skillSystem.ResetSkills();
        sceneState.ResetStates();
        enemySystem.ResetEnemies();
        companionSystem.companionECharacter = "swordsman";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
