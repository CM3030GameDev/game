using UnityEngine;

public class MainWeapon : MonoBehaviour
{
    [SerializeField] private PlayerAim playerAim;
    [SerializeField] private CharacterStats characterStats;   // for stat bonuses like attack s
    [SerializeField] private WeaponTier currentTier;          // starts as Pistol asset (changes based on the current tier)
    [SerializeField] private Transform gunSprite;
    [SerializeField] private SpriteRenderer gunRenderer;

    private float fireTimer;
    private int muzzleIndex;   // for alternating dual fire (dual pistols etc.)

    private void Update()
    {
        fireTimer += Time.deltaTime;

        // Effective interval: tier base minus the character's attackSpeed bonus
        float interval = Mathf.Max(0.05f, currentTier.fireInterval - characterStats.attackSpeed);

        if (fireTimer >= interval)
        {
            Fire();
            fireTimer = 0f;
        }

        if (gunSprite != null)
        {
            Vector2 aim = playerAim.AimDirection;
            float aimAngle = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
            gunSprite.rotation = Quaternion.AngleAxis(aimAngle, Vector3.forward);

            if (gunRenderer != null)
                gunRenderer.flipY = (aim.x < 0f);
        }
    }

    private void Fire()
    {
        Vector2 aim = playerAim.AimDirection;

        // Alternating to pick the next muzzle of the gun
        Vector2 offset = currentTier.muzzleOffsets[muzzleIndex];
        muzzleIndex = (muzzleIndex + 1) % currentTier.muzzleOffsets.Length;

        // Apply random spread to the weapon
        float spread = currentTier.spreadAngle;
        float angleOffset = (spread > 0f) ? Random.Range(-spread, spread) : 0f;
        Vector2 dir = Rotate(aim, angleOffset);

        // Position of weapon based off player position and muzzle offset rotated to aim
        Vector3 spawnPos = transform.position + (Vector3)RotateOffset(offset, aim);
        float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        GameObject b = Instantiate(currentTier.bulletPrefab, spawnPos,
                                   Quaternion.AngleAxis(rotZ, Vector3.forward));

        var rb = b.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = dir * currentTier.bulletSpeed;

        var bullet = b.GetComponent<Bullet>();
        if (bullet != null) bullet.SetDamage(currentTier.damage);
    }

    // Upgrade card will call this function to upgrade weapons
    public void SetTier(WeaponTier tier)
    {
        currentTier = tier;
        muzzleIndex = 0;
    }

    private Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad), sin = Mathf.Sin(rad);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
    }

    // Rotate muzzle offset
    private Vector2 RotateOffset(Vector2 offset, Vector2 aim)
    {
        float aimAngle = Mathf.Atan2(aim.y, aim.x);
        float cos = Mathf.Cos(aimAngle), sin = Mathf.Sin(aimAngle);
        return new Vector2(offset.x * cos - offset.y * sin,
                           offset.x * sin + offset.y * cos);
    }
}