using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobManager : MonoBehaviour
{
    [System.Serializable]
    public struct EnemySetup
    {
        public EnemyTypes type;
        public GameObject prefab;
        public int poolAmount;
    }

    private Transform playerTransform;
    public LayerMask groundLayer;

    //temporary serialized, for testing
    [SerializeField]
    private int killCount = 0;

    [Header("MinMax spawn distance")]
    [SerializeField] private float minSpawnDistance = 18f;
    [SerializeField] private float maxSpawnDistance = 20f;

    [Header("Prefab Mapping")]
    public List<EnemySetup> enemyPoolConfig = new List<EnemySetup>();

    private Dictionary<string, Coroutine> coroutines = new Dictionary<string, Coroutine>();
    private Dictionary<EnemyTypes, List<GameObject>> pooledEnemies = new Dictionary<EnemyTypes, List<GameObject>>();

    public enum EnemyTypes
    {
        None,
        REDMOB,
        BLUEMOB,
        GREENMOB,
        ROBOTMOB,
        ACT2BOSS,
        ACT3BOSS
    }
    public static MobManager Instance { get; private set; }

    private void Awake()
    {
        // Not DontDestroyOnLoad on purpose, since this holds scene specific state and kill count.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        //Reference to character scene object
        playerTransform = GameObject.FindGameObjectsWithTag("Character")[0].transform;

        //Initialising the object pool
        foreach (EnemySetup enemySetup in enemyPoolConfig)
        {
            List<GameObject> tempGameObjectList = new List<GameObject>();
            for (int i = 0; i < enemySetup.poolAmount; i++)
            {
                GameObject tempEnemy = Instantiate(enemySetup.prefab, gameObject.transform);
                Mob mobScript = tempEnemy.GetComponent<Mob>();
                if (mobScript != null)
                {
                    tempEnemy.GetComponent<Mob>().onDeath.AddListener(UpdateKillCount);
                }
                tempEnemy.SetActive(false);
                tempGameObjectList.Add(tempEnemy);
            }

            pooledEnemies.Add(enemySetup.type, tempGameObjectList);
        }
    }

    private void Start()
    {

    }

    private void OnDestroy()
    {
        // Clear the static so the next scene's manager can claim it.
        if (Instance == this) Instance = null;
    }

    /// <summary>
    /// Keeps roughly targetPopulation alive, topping up after kills instead of spawning batches.
    /// </summary>
    public void AddPopulationSpawnCoroutine(string coroutineName, EnemyTypes types, int targetPopulation, float checkInterval = 0.5f, Transform location = null)
    {
        Coroutine c = StartCoroutine(PopulationSpawnLoop(coroutineName, types, targetPopulation, checkInterval, location));
        coroutines.Add(coroutineName, c);
        Debug.Log(coroutineName + " has been added.");
    }

    private IEnumerator PopulationSpawnLoop(string coroutineName, EnemyTypes types, int targetPopulation, float checkInterval, Transform location)
    {
        while (true)
        {
            if (CountActive(types) < targetPopulation)
            {
                GameObject availableEnemy = FindAvaliableEnemyOfType(types);
                if (availableEnemy != null) SpawnEnemy(availableEnemy, location);
            }
            yield return new WaitForSeconds(checkInterval);
        }
    }

    private int CountActive(EnemyTypes types)
    {
        if (!pooledEnemies.ContainsKey(types)) return 0;
        int count = 0;
        foreach (GameObject enemy in pooledEnemies[types])
        {
            if (enemy.activeSelf) count++;
        }
        return count;
    }

    /// <summary>
    /// Use this function to start a coroutine that constantly spawns mobs until you tell it to stop.
    /// </summary>
    /// <param name="coroutineName">Name of the coroutine. Make sure it's unique.</param>
    /// <param name="delay">The time between each mob spawnning. Lower delay=faster spawning</param>
    /// <param name="types">Enum of enemy type.</param>
    /// <param name="location">The Transform of the location you want the mobs to spawn from.</param>
    /// <param name="spawnAmount">The number of enemies to spawn</param>
    public void AddSpawnCoroutine(string coroutineName, float delay, EnemyTypes types, int spawnAmount = -1, Transform location = null, string groundName = null)
    {
        Coroutine c = StartCoroutine(ConstantSpawnLoop(coroutineName, delay, types, spawnAmount, location, groundName));
        coroutines.Add(coroutineName, c);
        //Debug.Log(coroutineName + " has been added.");
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

            //Debug.Log(coroutineName + " has been stopped.");
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
        Debug.Log("All spawners have been stopped." + coroutines.Count);
    }

    /// <summary>
    /// Spawns a batch of enemies, then waits until every one of that type is dead before
    /// spawning the next batch. Repeats forever until stopped. Batch size is randomized
    /// between minWaveSize and maxWaveSize (inclusive) each time.
    /// </summary>
    public void AddWaveSpawnCoroutine(string coroutineName, EnemyTypes types, int minWaveSize, int maxWaveSize, float spawnInterval = 0.15f, Transform location = null, string groundName = null)
    {
        Coroutine c = StartCoroutine(WaveSpawnLoop(coroutineName, types, minWaveSize, maxWaveSize, spawnInterval, location, groundName));
        coroutines.Add(coroutineName, c);
        Debug.Log(coroutineName + " has been added.");
    }

    private IEnumerator WaveSpawnLoop(string coroutineName, EnemyTypes types, int minWaveSize, int maxWaveSize, float spawnInterval, Transform location, string groundName)
    {
        while (true)
        {
            int waveSize = Random.Range(minWaveSize, maxWaveSize + 1);
            for (int i = 0; i < waveSize;)
            {
                GameObject availableEnemy = FindAvaliableEnemyOfType(types);
                if (availableEnemy != null)
                {
                    bool hasSpawned = SpawnEnemy(availableEnemy, location, groundName);
                    if (hasSpawned)
                        i++;
                }
                    

                yield return new WaitForSeconds(spawnInterval);
            }

            yield return new WaitUntil(() => AllOfTypeDead(types));
        }
    }

    private bool AllOfTypeDead(EnemyTypes types)
    {
        if (!pooledEnemies.ContainsKey(types)) return true;

        foreach (GameObject mob in pooledEnemies[types])
        {
            if (mob.activeSelf) return false;
        }
        return true;
    }

    private IEnumerator ConstantSpawnLoop(string coroutineName, float delayBetweenSpawns, EnemyTypes types, int spawnAmount = -1, Transform location = null, string groundName = null)
    {
        int spawnedCount = 0;
        while (spawnAmount < 0 || spawnedCount < spawnAmount)
        {
            GameObject availableEnemy = FindAvaliableEnemyOfType(types);
            if (availableEnemy != null)
            {
                bool hasSpawned = SpawnEnemy(availableEnemy, location, groundName);
                if (hasSpawned)
                    spawnedCount++;
            }

            yield return new WaitForSeconds(delayBetweenSpawns);
        }
        coroutines.Remove(coroutineName);
        //Debug.Log(coroutineName + " has been removed");
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

    public GameObject SpawnBoss(EnemyTypes type, Transform spawnPos)
    {
        GameObject avaliableBoss = FindAvaliableEnemyOfType(type);
        if (avaliableBoss != null)
        {
            avaliableBoss.transform.position = spawnPos.position;
            avaliableBoss.SetActive(true);
            return avaliableBoss;
        }

        return null;
    }

    public void DespawnBoss(EnemyTypes type)
    {
        if (!pooledEnemies.ContainsKey(type))
            return;

        foreach (GameObject enemy in pooledEnemies[type])
        {
            if (enemy.activeSelf)
                enemy.SetActive(false);
        }
    }

    private bool SpawnEnemy(GameObject enemyToSpawn, Transform location = null, string groundName = null)
    {
        float targetX;
        float targetY;

        if (location != null)
        {
            targetX = location.position.x;
            targetY = location.position.y;
        }
        else
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            float randomDistance = Random.Range(minSpawnDistance, maxSpawnDistance);
            targetX = playerTransform.position.x + (randomDirection.x * randomDistance);
            targetY = playerTransform.position.y + (randomDirection.y * randomDistance);
        }

        Vector3 raycastStartPos = new Vector3(targetX, targetY, -5f);

        RaycastHit2D hit = Physics2D.GetRayIntersection(new Ray(raycastStartPos, Vector3.forward), 10f, groundLayer);

        if (hit.collider != null)
        {
            if (groundName == null || groundName == "" || hit.collider.gameObject.name == groundName)
            {
                enemyToSpawn.transform.position = new Vector3(targetX, targetY, 0f);
                enemyToSpawn.SetActive(true);
                return true;
            }
            return false;
        }
        else
        {
            //Debug.Log("X:" + targetX + " Y: " + targetY + " has no ground");
            return false;
        }
    }

    /// <summary>
    /// Spawns one pooled enemy at an exact position.
    /// Intended for controlled/tutorial encounters where
    /// ground validation is not required.
    /// </summary>
    public GameObject SpawnEnemyAtPosition(
        EnemyTypes type,
        Vector3 position
    )
    {
        GameObject availableEnemy = FindAvaliableEnemyOfType(type);

        if (availableEnemy == null)
        {
            Debug.LogWarning(
                "No available pooled enemy of type: " + type
            );

            return null;
        }

        availableEnemy.transform.position = position;
        availableEnemy.SetActive(true);

        Debug.Log(
            "Enemy spawned directly at: " + position
        );

        return availableEnemy;
    }

    public void instantKillAllActive()
    {
        foreach (KeyValuePair<EnemyTypes, List<GameObject>> pool in pooledEnemies)
        {
            foreach (GameObject mob in pool.Value)
            {
                if (mob.activeSelf)
                {
                    mob.GetComponent<Mob>().Despawn();
                }
            }
        }
    }

    /// <summary>
    /// Checks if any mobs are still alive. Only use after stopping spawners.
    /// </summary>
    /// <returns>true if all mobs are dead</returns>
    public bool AreAllMobsDead()
    {
        foreach (KeyValuePair<EnemyTypes, List<GameObject>> pool in pooledEnemies)
        {
            foreach (GameObject mob in pool.Value)
            {
                if (mob.activeSelf)
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

    public void ResetKillCount()
    {
        killCount = 0;
    }

    public List<GameObject> GetAllPooledEnemies()
    {
        List<GameObject> allEnemies = new List<GameObject>();
        foreach (KeyValuePair<EnemyTypes, List<GameObject>> pool in pooledEnemies)
        {
            foreach (GameObject mob in pool.Value)
            {
                allEnemies.Add(mob);
            }
        }
        return allEnemies;
    }
}

//Important note
//Spawner only works if the ground has a collider. isTrigger is fine.
//Currently only works if raycast hits a Ground layermask.
//If need more, change variable to a list instead.
