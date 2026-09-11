using UnityEngine;

public class ActThreeEnemies : MonoBehaviour
{
    [SerializeField] private ActThreeMobCount actThreeMobCount;
    [SerializeField] private Transform bossPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Reset mob count to a maximum of 30 robot mobs that can spawn in Act 3 phase one and phase two respectively
        actThreeMobCount.ResetMobCount();
        //Spawn up to 10 maximum robot mobs at any point in time on the map
        MobManager.Instance.AddPopulationSpawnCoroutine("spawn", MobManager.EnemyTypes.ROBOTMOB, 10);
        //Spawn boss
        MobManager.Instance.SpawnBoss(MobManager.EnemyTypes.ACT3BOSS, bossPos);
    }

    // Update is called once per frame
    void Update()
    {
        //Stop spawning robot mobs after 30 robot mobs are destroyed
        if (actThreeMobCount.mobCount <= 0)
        {
            MobManager.Instance.StopAllSpawnCoroutines();
        }
    }
}
