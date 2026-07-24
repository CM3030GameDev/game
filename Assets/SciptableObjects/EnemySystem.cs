using UnityEngine;

[CreateAssetMenu(fileName = "EnemySystem", menuName = "Scriptable Objects/EnemySystem")]
public class EnemySystem : ScriptableObject
{
    public int enemyLeft = 0;
    public int currentWave = 1;
    public bool enemySpawn = false;

    //Reset to default
    public void ResetEnemies()
    {
        enemyLeft = 0;
    }
}
