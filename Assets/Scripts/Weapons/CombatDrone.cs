using System.Collections.Generic;
using UnityEngine;

// One orbiting drone. DroneWeapon drives its position and lifetime, so this only deals damage.
// The collider is not used for triggers, it only defines the hit radius.
[RequireComponent(typeof(CircleCollider2D))]
public class CombatDrone : MonoBehaviour
{
    [Tooltip("How long the enemy flashes when this drone hits it.")]
    [SerializeField] private float hitFlash = 0.15f;

    private DroneWeapon owner;
    private float hitCooldown;
    private CircleCollider2D col;

    // Cooldown is per drone and per enemy, so drones never skip mobs or block each other.
    private readonly Dictionary<Component, float> nextHitTime = new Dictionary<Component, float>();   // mob or boss

    private static readonly Collider2D[] Hits = new Collider2D[16];

    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();
    }

    public void Configure(DroneWeapon weapon, float cooldown)
    {
        owner = weapon;
        hitCooldown = cooldown;
    }

    // Checked every frame, since an orbiting drone sampled once per cooldown mostly finds nothing.
    private void Update()
    {
        if (owner == null) return;

        // lossyScale, because the prefab is scaled and the collider radius is local.
        float radius = col.radius * Mathf.Abs(transform.lossyScale.x);
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, radius, Hits, BossDamage.Mask);

        for (int i = 0; i < count; i++)
        {
            Mob mob = Hits[i].GetComponent<Mob>();
            Component target = mob != null ? mob : BossDamage.Find(Hits[i]);
            if (target == null) continue;

            // Keyed per target, which also stops a boss's extra colliders from being hit twice.
            if (nextHitTime.TryGetValue(target, out float ready) && Time.time < ready) continue;
            nextHitTime[target] = Time.time + hitCooldown;

            if (mob != null) mob.UnblockedAttack(owner.DroneDamage, hitFlash);
            else BossDamage.Damage((MonoBehaviour)target, owner.DroneDamage, hitFlash);
        }
    }
}
