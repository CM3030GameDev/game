using UnityEngine;
using System.Collections.Generic;

public class EnemyWave : MonoBehaviour
{
    //[SerializeField] private GameObject character;
    [SerializeField] private EnemySystem enemySystem;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject character;
    [SerializeField] private float enemyDistance = 20f;
    private List<GameObject> enemyPool = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemySystem.currentWave += 1;
        for(int i = 0; i < 20; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(enemySystem.enemyLeft == 0)
        {
            enemySystem.currentWave += 1;
            enemySystem.enemyLeft = 20;
            for(int i = 0; i < enemyPool.Count; i++)
            {
                float randomNum = Random.Range(0f, 4f);
                if(randomNum < 1f)
                {
                    enemyPool[i].transform.position = character.transform.position + new Vector3(enemyDistance * -1, Random.Range(-enemyDistance, enemyDistance), 0);
                }
                else if(randomNum < 2f)
                {
                    enemyPool[i].transform.position = character.transform.position + new Vector3(enemyDistance, Random.Range(-enemyDistance, enemyDistance), 0);
                }
                else if (randomNum < 3f)
                {
                    enemyPool[i].transform.position = character.transform.position + new Vector3(Random.Range(-enemyDistance, enemyDistance), enemyDistance * -1, 0);
                }
                else if (randomNum < 4f)
                {
                    enemyPool[i].transform.position = character.transform.position + new Vector3(Random.Range(-enemyDistance, enemyDistance), enemyDistance, 0);
                }
                enemyPool[i].SetActive(true);
            }
        }
    }
}
