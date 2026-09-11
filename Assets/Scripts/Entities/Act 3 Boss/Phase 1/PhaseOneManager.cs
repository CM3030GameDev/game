using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhaseOneManager : MonoBehaviour
{
    public Transform characterTransform;
    public Character character;
    public GameObject fogs;
    public Slider slider;
    public TextMeshProUGUI tmp;
    //Gameobject pool for missile barrage attacks
    public Queue<GameObject> missiles;
    //Gameobject pool for fire cannon attacks
    public Queue<GameObject> explosions;
    //Gameobject pool for indicator marks
    public Queue<GameObject> indicators;

    [SerializeField] private ActThreeMobCount actThreeMobCount;
    [SerializeField] private Transform bossPos;

    [Header("Boss Attacks")]
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject indicatorPrefab;

    //Create singleton instance
    public static PhaseOneManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        missiles = new Queue<GameObject>();
        explosions = new Queue<GameObject>();
        indicators = new Queue<GameObject>();

        for(int i = 0; i < 40; i++)
        {
            //Create new missile prefab
            GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            //Disable new missile prefab
            missile.SetActive(false);
            //Add new missile prefab to object pool
            missiles.Enqueue(missile);

            //Create new indicator prefab
            GameObject indicator = Instantiate(indicatorPrefab, transform.position, Quaternion.identity);
            //Disable new indicator prefab
            indicator.SetActive(false);
            //Add new indicator prefab to object pool
            indicators.Enqueue(indicator);

            if (i < 30)
            {
                //Create new explosion prefab
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                //Disable new explosion prefab
                explosion.SetActive(false);
                //Add new explosion prefab to object pool
                explosions.Enqueue(explosion);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Reset mob count
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
