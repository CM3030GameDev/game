using UnityEngine;
using System.Collections.Generic;

public class Mobs : MonoBehaviour
{
    [SerializeField] private EnemySystem enemySystem;
    [SerializeField] private SceneState sceneState;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject spawnPos1;
    [SerializeField] private GameObject spawnPos2;
    [SerializeField] private float enemyDistance = 20f;
    public List<GameObject> enemies = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < 30; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemies.Add(enemy);
        }
        enemySystem.enemyLeft = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if(sceneState.act == 1)
        {
            if(!enemySystem.enemySpawn)
            {
                foreach (GameObject enemy in enemies)
                {
                    float randomNum = Random.Range(0f, 1f);
                    if (randomNum <= 0.5f)
                    {
                        enemy.transform.position = spawnPos1.transform.position + new Vector3(enemyDistance * -1, Random.Range(-enemyDistance, enemyDistance), 0);
                    }
                    else
                    {
                        enemy.transform.position = spawnPos2.transform.position + new Vector3(enemyDistance, Random.Range(-enemyDistance, enemyDistance), 0);
                    }
                    enemy.SetActive(true);
                }
                enemySystem.enemySpawn = true;
            }
            else if (enemySystem.enemyLeft <= 20)
            {
                foreach (GameObject enemy in enemies)
                {
                    if (!enemy.activeSelf)
                    {
                        float randomNum = Random.Range(0f, 1f);
                        if (randomNum <= 0.5f)
                        {
                            enemy.transform.position = spawnPos1.transform.position + new Vector3(enemyDistance * -1, Random.Range(-enemyDistance, enemyDistance), 0);
                        }
                        else
                        {
                            enemy.transform.position = spawnPos2.transform.position + new Vector3(enemyDistance, Random.Range(-enemyDistance, enemyDistance), 0);
                        }
                        enemy.SetActive(true);
                        enemySystem.enemyLeft += 1;
                    }
                }
            }
        }
        else
        {
            if(enemySystem.enemyRespawn)
            {
                if (!enemySystem.enemySpawn)
                {
                    foreach (GameObject enemy in enemies)
                    {
                        float randomNum = Random.Range(0f, 1f);
                        if (randomNum <= 0.5f)
                        {
                            enemy.transform.position = spawnPos1.transform.position + new Vector3(enemyDistance * -1, Random.Range(-enemyDistance, enemyDistance), 0);
                        }
                        else
                        {
                            enemy.transform.position = spawnPos2.transform.position + new Vector3(enemyDistance, Random.Range(-enemyDistance, enemyDistance), 0);
                        }
                        enemy.SetActive(true);
                    }
                    enemySystem.enemySpawn = true;
                }
                else if (enemySystem.enemyLeft <= 20)
                {
                    foreach (GameObject enemy in enemies)
                    {
                        if (!enemy.activeSelf)
                        {
                            float randomNum = Random.Range(0f, 1f);
                            if (randomNum <= 0.5f)
                            {
                                enemy.transform.position = spawnPos1.transform.position + new Vector3(enemyDistance * -1, Random.Range(-enemyDistance, enemyDistance), 0);
                            }
                            else
                            {
                                enemy.transform.position = spawnPos2.transform.position + new Vector3(enemyDistance, Random.Range(-enemyDistance, enemyDistance), 0);
                            }
                            enemy.SetActive(true);
                            enemySystem.enemyLeft += 1;
                        }
                    }
                }
            }
        }
    }
}
