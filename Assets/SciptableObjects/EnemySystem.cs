using UnityEngine;

[CreateAssetMenu(fileName = "EnemySystem", menuName = "Scriptable Objects/EnemySystem")]
public class EnemySystem : ScriptableObject
{
    public bool enemySpawn = false;
    public int enemyLeft = 0;
    public bool enemyRespawn = true;

    //Reset to default
    public void ResetEnemies()
    {
        enemySpawn = false;
        enemyLeft = 0;
        enemyRespawn = true;
    }
}
