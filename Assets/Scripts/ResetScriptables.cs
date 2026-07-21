using UnityEngine;

public class ResetScriptables : MonoBehaviour
{
    [SerializeField] private CompanionSystem companionSystem;
    [SerializeField] private WeaponSystem weaponSystem;
    [SerializeField] private SceneState sceneState;
    [SerializeField] private EnemySystem enemySystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        companionSystem.ResetCompanions();
        weaponSystem.ResetWeapons();
        sceneState.ResetStates();
        enemySystem.ResetEnemies();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
