using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobtest : MonoBehaviour
{
    [System.Serializable]
    public struct EnemySetup
    {
        public EnemyTypes type;
        public GameObject prefab;
        public int poolAmount;
    }

    public Transform playerTransform;
    public LayerMask groundLayer;

    //temporary serialized, for testing
    [SerializeField]
    private int killCount = 0;

    [Header("MinMax spawn distance")]
    [SerializeField] private float minSpawnDistance = 18f; // Must be larger than half your screen width
    [SerializeField] private float maxSpawnDistance = 20f;

    [Header("Prefab Mapping")]
    public List<EnemySetup> enemyPoolConfig = new List<EnemySetup>();

    private Dictionary<string, Coroutine> coroutines = new Dictionary<string, Coroutine>();
    private Dictionary<EnemyTypes, List<GameObject>> pooledEnemies = new Dictionary<EnemyTypes, List<GameObject>>();

    public enum EnemyTypes
    {
        None,
        AAA,
        BBB,
        CCC
    }
    public static mobtest Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        //Initialising the object pool
        foreach(EnemySetup enemySetup in enemyPoolConfig)
        {
            List<GameObject> tempGameObjectList = new List<GameObject>();
            for(int i = 0; i < enemySetup.poolAmount; i++)
            {
                GameObject tempEnemy = Instantiate(enemySetup.prefab);
                tempEnemy.SetActive(false);
                tempGameObjectList.Add(tempEnemy);
            }

            pooledEnemies.Add(enemySetup.type, tempGameObjectList);
        }

        //test
        //addCoroutine("aa", 1f, EnemyTypes.AAA);
        //addCoroutine("bb", 5f, EnemyTypes.BBB);
    }
    
    /// <summary>
    /// Use this function to start a coroutine that constantly spawns mobs until you tell it to stop.
    /// </summary>
    /// <param name="coroutineName">Name of the coroutine. Make sure it's unique.</param>
    /// <param name="delay">The time between each mob spawnning. Lower delay=faster spawning</param>
    /// <param name="types">Enum of enemy type.</param>
    public void AddSpawnCoroutine(string coroutineName, float delay, EnemyTypes types)
    {
        Coroutine c = StartCoroutine(ConstantSpawnLoop(delay, types));
        coroutines.Add(coroutineName, c);
    }

    /// <summary>
    /// Use this function to start a coroutine that constantly spawns mobs until you tell it to stop.
    /// </summary>
    /// <param name="coroutineName">Name of the coroutine. Make sure it's unique.</param>
    /// <param name="delay">The time between each mob spawnning. Lower delay=faster spawning</param>
    /// <param name="types">Enum of enemy type.</param>
    /// <param name="location">The Transform of the location you want the mobs to spawn from.</param>
    public void AddSpawnCoroutine(string coroutineName, float delay, EnemyTypes types, Transform location)
    {
        Coroutine c = StartCoroutine(ConstantSpawnLoop(delay, types, location));
        coroutines.Add(coroutineName, c);
    }

    /// <summary>
    /// Use this function to stop a spawner coroutine.
    /// </summary>
    /// <param name="coroutineName">The name of the coroutine you want to stop. Make sure it is the same name from "addCoroutine".</param>
    public void StopSpawnCoroutine(string coroutineName)
    {
        if (coroutines.ContainsKey(coroutineName))
        {
            StopCoroutine(coroutines[coroutineName]);
            coroutines.Remove(coroutineName);

            Debug.Log(coroutineName + " has been stopped.");
        }
    }

    /// <summary>
    /// Stops all spawner coroutines.
    /// </summary>
    public void StopAllSpawnCoroutines()
    {
        foreach (KeyValuePair<string, Coroutine> c in coroutines)
        {
            StopCoroutine(c.Value);
        }
        coroutines.Clear();
        Debug.Log("All spawners have been stopped.");
    }

    private IEnumerator ConstantSpawnLoop(float delayBetweenSpawns, EnemyTypes types)
    {
        while (true)
        {
            GameObject availableEnemy = FindAvaliableEnemyOfType(types);
            if (availableEnemy != null)
            {
                SpawnEnemy(availableEnemy);
            }

            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }

    private IEnumerator ConstantSpawnLoop(float delayBetweenSpawns, EnemyTypes types, Transform location)
    {
        while (true)
        {
            GameObject availableEnemy = FindAvaliableEnemyOfType(types);
            if (availableEnemy != null)
            {
                SpawnEnemy(availableEnemy, location);
            }

            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }

    private GameObject FindAvaliableEnemyOfType(EnemyTypes types)
    {
        if (!pooledEnemies.ContainsKey(types))
            return null;

        foreach (GameObject enemy in pooledEnemies[types])
        {
            if (enemy.activeSelf == false)
                return enemy;
        }
        return null;
    }
    void SpawnEnemy(GameObject enemyToSpawn)
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
        float targetX = playerTransform.position.x + (randomDirection.x * randomDistance);
        float targetY = playerTransform.position.y + (randomDirection.y * randomDistance);

        Vector3 raycastStartPos = new Vector3(targetX, targetY, -5f);

        RaycastHit2D hit = Physics2D.GetRayIntersection(new Ray(raycastStartPos, Vector3.forward), 10f, groundLayer);

        if (hit.collider != null)
        {
            enemyToSpawn.transform.position = new Vector3(targetX, targetY, 0f);
            enemyToSpawn.SetActive(true);
        }
        else
        {
            Debug.Log("X:" + targetX + " Y: " + targetY + " has no ground");
        }
    }

    void SpawnEnemy(GameObject enemyToSpawn, Transform location)
    {
        float targetX = location.position.x;
        float targetY = location.position.y;

        Vector3 raycastStartPos = new Vector3(targetX, targetY, -5f);

        RaycastHit2D hit = Physics2D.GetRayIntersection(new Ray(raycastStartPos, Vector3.forward), 10f, groundLayer);

        if (hit.collider != null)
        {
            enemyToSpawn.transform.position = new Vector3(targetX, targetY, 0f);
            enemyToSpawn.SetActive(true);
        }
        else
        {
            Debug.Log("X:" + targetX + " Y: " + targetY + " has no ground");
        }
    }

    /// <summary>
    /// Checks if any mobs are still alive. Only use after stopping spawners.
    /// </summary>
    /// <returns>true if all mobs are dead</returns>
    public bool AreAllMobsDead()
    {
        foreach(KeyValuePair<EnemyTypes, List<GameObject>> pool in pooledEnemies)
        {
            foreach(GameObject mob  in pool.Value)
            {
                if(mob.activeSelf)
                {
                    return false;
                }
            }
        }

        return true;
    }
    //maybe just make all the mobs die when generator go kaboom

    //call this when mob dies(not used yet have to touch mob script)
    public void UpdateKillCount()
    {
        killCount++;
    }

    public int GetKillCount()
    {
        return killCount;
    }

    public void SpawnBoss(EnemyTypes enemyType, Transform location)
    {

    }
}

//Important note
//Spawner only works if the ground has a collider. isTrigger is fine.
//Currently only works if raycast hits a Ground layermask.
//If need more, change variable to a list instead.

//DO SPAWNER FOR BOSSES