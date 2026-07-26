using UnityEngine;
using System.Collections.Generic;

public class EnemyWave : MonoBehaviour
{
    //[SerializeField] private GameObject character;
    [SerializeField] private EnemySystem enemySystem;
    [SerializeField] private SceneState sceneState;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject spawnPos1;
    [SerializeField] private GameObject spawnPos2;
    [SerializeField] private float enemyDistance = 20f;
    private List<GameObject> enemies = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemySystem.currentWave = 1;
        enemySystem.enemyLeft = 0;
        for(int i = 0; i < 20; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemies.Add(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(enemySystem.enemyLeft == 0 && sceneState.act == 1 && enemySystem.currentWave <= 5)
        {
            enemySystem.currentWave += 1;
            enemySystem.enemyLeft = 20;
            for (int i = 0; i < enemies.Count; i++)
            {
                float randomNum = Random.Range(0f, 1f);
                if (randomNum <= 0.5f)
                {
                    enemies[i].transform.position = spawnPos1.transform.position + new Vector3(enemyDistance * -1, Random.Range(-enemyDistance, enemyDistance), 0);
                }
                else
                {
                    enemies[i].transform.position = spawnPos2.transform.position + new Vector3(enemyDistance, Random.Range(-enemyDistance, enemyDistance), 0);
                }
                enemies[i].SetActive(true);
            }
        }
        else if(enemySystem.enemyLeft == 0 && sceneState.act == 2)
        {
            enemySystem.currentWave += 1;
            enemySystem.enemyLeft = 20;
            for (int i = 0; i < enemies.Count; i++)
            {
                float randomNum = Random.Range(0f, 1f);
                if (randomNum <= 0.5f)
                {
                    enemies[i].transform.position = spawnPos1.transform.position + new Vector3(enemyDistance * -1, Random.Range(-enemyDistance, enemyDistance), 0);
                }
                else
                {
                    enemies[i].transform.position = spawnPos2.transform.position + new Vector3(enemyDistance, Random.Range(-enemyDistance, enemyDistance), 0);
                }
                enemies[i].SetActive(true);
            }
        }
    }
}
