using UnityEngine;

// One orbiting drone. Positioning is driven by DroneWeapon; this only handles
// damage-on-contact and (for the non-combined form) expiry.
[RequireComponent(typeof(Collider2D))]
public class CombatDrone : MonoBehaviour
{
    private DroneWeapon owner;
    private float hitCooldown;
    private float cooldownTimer;

    private static int EnemyLayer;

    private void Awake() => EnemyLayer = LayerMask.NameToLayer("Enemy");

    public void Configure(DroneWeapon weapon, float cooldown, float lifetime)
    {
        owner = weapon;
        hitCooldown = cooldown;
        if (lifetime > 0f) Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (cooldownTimer > 0f) cooldownTimer -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (cooldownTimer > 0f || owner == null) return;
        if (other.gameObject.layer != EnemyLayer) return;

        Mob mob = other.GetComponent<Mob>();
        if (mob == null) return;

        mob.MobAttacked(owner.DroneDamage, 0.05f);
        cooldownTimer = hitCooldown;
    }
}
