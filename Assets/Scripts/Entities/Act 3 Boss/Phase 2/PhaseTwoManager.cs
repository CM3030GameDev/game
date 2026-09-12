using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhaseTwoManager : MonoBehaviour
{
    public Transform characterTransform;
    public Character character;
    public Slider slider;
    public TextMeshProUGUI tmp;
    //Gameobject pool for homing missile attacks
    public Queue<GameObject> missiles;
    //Gameobject pool for ground impact attacks
    public Queue<GameObject> impacts;
    //Gameobject pool for elemental projectile attacks
    public Queue<GameObject> projectiles;
    //Gameobject pool for indicator marks
    public Queue<GameObject> indicators;

    [SerializeField] private ActThreeMobCount actThreeMobCount;
    [SerializeField] private Transform bossPos;

    [Header("Boss Attacks")]
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private GameObject impactPrefab;
    [SerializeField] private GameObject windPrefab;
    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private GameObject indicatorPrefab;

    [Header("Status Effects")]
    [SerializeField] private GameObject stunned;
    [SerializeField] private GameObject confusion;
    [SerializeField] private GameObject burnt;


    //Create singleton instance
    public static PhaseTwoManager Instance { get; private set; }

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
        impacts = new Queue<GameObject>();
        projectiles = new Queue<GameObject>();
        indicators = new Queue<GameObject>();

        //Create new wind projectile prefab
        GameObject windProjectile = Instantiate(windPrefab, transform.position, Quaternion.identity);
        //Disable new wind projectile prefab
        windProjectile.SetActive(false);
        //Add new wind projectile prefab to object pool
        projectiles.Enqueue(windProjectile);

        //Create new lightning projectile prefab
        GameObject lightningProjectile = Instantiate(lightningPrefab, transform.position, Quaternion.identity);
        //Disable new lightning projectile prefab
        lightningProjectile.SetActive(false);
        //Add new lightning projectile prefab to object pool
        projectiles.Enqueue(lightningProjectile);

        //Create new fire projectile prefab
        GameObject fireProjectile = Instantiate(firePrefab, transform.position, Quaternion.identity);
        //Disable new fire projectile prefab
        fireProjectile.SetActive(false);
        //Add new fire projectile prefab to object pool
        projectiles.Enqueue(fireProjectile);

        for (int i = 0; i < 10; i++)
        {
            //Create new missile prefab
            GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            //Disable new missile prefab
            missile.SetActive(false);
            //Add new missile prefab to object pool
            missiles.Enqueue(missile);

            //Create new impact prefab
            GameObject impact = Instantiate(impactPrefab, transform.position, Quaternion.identity);
            //Disable new impact prefab
            impact.SetActive(false);
            //Add new impact prefab to object pool
            impacts.Enqueue(impact);

            //Create new indicator prefab
            GameObject indicator = Instantiate(indicatorPrefab, transform.position, Quaternion.identity);
            //Disable new indicator prefab
            indicator.SetActive(false);
            //Add new indicator prefab to object pool
            indicators.Enqueue(indicator);
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

    public void Stunned()
    {
        stunned.SetActive(true);
    }

    public void Confusion()
    {
        confusion.SetActive(true);
    }

    public void Burnt()
    {
        burnt.SetActive(true);
    }
}
