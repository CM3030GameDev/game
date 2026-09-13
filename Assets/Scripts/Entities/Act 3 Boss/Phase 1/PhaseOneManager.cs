using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhaseOneManager : MonoBehaviour
{
    public Transform characterTransform;
    public Character character;
    public Transform doorTransform;
    public BoxCollider2D doorCollider;
    public GameObject fogs;
    public Slider slider;
    public TextMeshProUGUI tmp;
    public MissionUI missionUI;
    public bool hydrantHint;

    [Header("References")]
    [SerializeField] private ActThreeMobCount actThreeMobCount;
    [SerializeField] private Transform bossPos;

    [Header("Object Pools")]
    //Gameobject pool for missile barrage attacks
    public Queue<GameObject> missiles;
    //Gameobject pool for fire cannon attacks
    public Queue<GameObject> explosions;
    //Gameobject pool for indicator marks
    public Queue<GameObject> indicators;

    [Header("Attack List")]
    //List for all attacks (To be used for disabling all concurrent attacks when Villain die)
    public List<GameObject> attackList;

    [Header("Boss Attacks")]
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject indicatorPrefab;

    [Header("Mission Header")]
    [SerializeField] private string header;

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

        //Resume timescale
        Time.timeScale = 1f;

        //Initialise no hydrant to be broken yet
        hydrantHint = false;

        missiles = new Queue<GameObject>();
        explosions = new Queue<GameObject>();
        indicators = new Queue<GameObject>();
        attackList = new List<GameObject>();

        for(int i = 0; i < 40; i++)
        {
            //Create new missile prefab
            GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            //Disable new missile prefab
            missile.SetActive(false);
            //Add new missile prefab to object pool
            missiles.Enqueue(missile);
            //Add missile to attack list
            attackList.Add(missile);

            //Create new indicator prefab
            GameObject indicator = Instantiate(indicatorPrefab, transform.position, Quaternion.identity);
            //Disable new indicator prefab
            indicator.SetActive(false);
            //Add new indicator prefab to object pool
            indicators.Enqueue(indicator);
            //Add indicator to attack list
            attackList.Add(indicator);

            if (i < 20)
            {
                //Create new explosion prefab
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                //Disable new explosion prefab
                explosion.SetActive(false);
                //Add new explosion prefab to object pool
                explosions.Enqueue(explosion);
                //Add explosion to list
                attackList.Add(explosion);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Display mission header for Act 3 Phase One
        missionUI.SetHeader(header);
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

    }

    //Point quest arrow towards door position after Villain has died
    public void ArrowPointer()
    {
        missionUI.SetArrowTarget(doorTransform);
    }
}
