using UnityEngine;
using System.Collections;

public class CompanionSkills : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Character character;
    [SerializeField] private Transform mercenary;
    [SerializeField] private Transform swordsman;
    [SerializeField] private Mobs mobs;

    [Header("Mercenary: Fire Floor")]
    [SerializeField] private GameObject fireFloorPrefab;
    [SerializeField] private float mercCooldown = 12f;
    [SerializeField] private float mercCastTime = 0.4f;

    [Header("Swordsman: Deflect / Invuln")]
    [SerializeField] private float invulnDuration = 3f;
    [SerializeField] private float swordCooldown = 15f;
    [SerializeField] private float swordCastTime = 0.4f;

    private float mercTimer;
    private float swordTimer;
    private bool casting;

    public bool MercReady => mercTimer <= 0f;
    public bool SwordReady => swordTimer <= 0f;

    private void Update()
    {
        if (mercTimer > 0f) mercTimer -= Time.deltaTime;
        if (swordTimer > 0f) swordTimer -= Time.deltaTime;

        if (casting) return;   // Prevent more than one skill from being cast at the same time

        if (Input.GetKeyDown(KeyCode.Alpha1) && MercReady)
        {
            StartCoroutine(MercSkill());
            mercTimer = mercCooldown;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && SwordReady)
        {
            StartCoroutine(SwordSkill());
            swordTimer = swordCooldown;
        }
    }

    private IEnumerator MercSkill()
    {
        casting = true;

        Vector3 origin = mercenary.position;
        mercenary.position = transform.position;      // Teleport to the player
        yield return new WaitForSeconds(mercCastTime); // Brief cast time delay

        // Fire floor lands on the densest enemy cluster
        Vector2 target = FindEnemyCluster();
        Instantiate(fireFloorPrefab, target, Quaternion.identity);

        casting = false;
    }

    private IEnumerator SwordSkill()
    {
        casting = true;

        swordsman.position = transform.position;       // teleport to the player
        yield return new WaitForSeconds(swordCastTime);

        character.GrantInvulnerability(invulnDuration); // protect the player (for swordsman skill)

        casting = false;
    }

    // Find the position with the most enemies nearby
    private Vector2 FindEnemyCluster()
    {
        Vector2 best = transform.position;
        int bestCount = -1;

        foreach (var e in mobs.enemies)
        {
            if (!e.activeInHierarchy) continue;

            int count = 0;
            foreach (var other in mobs.enemies)
            {
                if (!other.activeInHierarchy) continue;
                if (Vector2.Distance(e.transform.position, other.transform.position) < 3f)
                    count++;
            }

            if (count > bestCount)
            {
                bestCount = count;
                best = e.transform.position;
            }
        }

        return best;
    }
}