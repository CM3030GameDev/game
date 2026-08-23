using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoldierSkill : MonoBehaviour
{
    [Header("Cooldown")]
    [SerializeField] private float cooldown = 8f;

    [Header("Airstrike")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private int strikeCount = 10;    // Number of airstrikes 
    [SerializeField] private float strikeInterval = 0.8f;  // Delay between each airstrike
    [SerializeField] private float scatter = 0.8f;   // RandomOffset for each airstrike

    private List<GameObject> allEnemies;

    private float cooldownTimer;

    public bool IsReady => cooldownTimer <= 0f;
    public float CooldownRemaining => Mathf.Max(0f, cooldownTimer);
    public float CooldownDuration => cooldown;

    private void Start()
    {
        allEnemies = MobManager.Instance.GetAllPooledEnemies();
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(1) && IsReady)
        {
            StartCoroutine(Airstrike());
            cooldownTimer = cooldown;
        }
    }

    private IEnumerator Airstrike()
    {
        for (int i = 0; i < strikeCount; i++)
        {
            Vector2 target = PickTarget();
            // Small scatter so it looks less targeted
            target += new Vector2(Random.Range(-scatter, scatter),
                                  Random.Range(-scatter, scatter));

            Instantiate(explosionPrefab, target, Quaternion.identity);

            yield return new WaitForSeconds(strikeInterval);
        }
    }

    private Vector2 PickTarget()
    {
        // Gather living enemies
        var alive = new System.Collections.Generic.List<GameObject>();
        foreach (var e in allEnemies)
            if (e.activeInHierarchy) alive.Add(e);

        // Hit a random enemy if any exist, otherwise scatter near the player
        if (alive.Count > 0)
            return alive[Random.Range(0, alive.Count)].transform.position;

        return (Vector2)transform.position +
               new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
    }

    public void ApplyUpgrade(SkillStat stat, float amount)
    {
        switch (stat)
        {
            case SkillStat.Cooldown: cooldown = Mathf.Max(2f, cooldown - amount); break;
            case SkillStat.StrikeCount: strikeCount += (int)amount; break;
        }
    }
}