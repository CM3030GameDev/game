using UnityEngine;

public class ShotgunWeapon : SecondaryWeapon
{
    [SerializeField] private GameObject pelletPrefab;
    [SerializeField] private float pelletSpeed = 20f;
    [SerializeField] private float knockback = 4f;

    protected override void Fire()
    {
        Vector2 baseDir = playerAim.AimDirection;
        int pellets = Mathf.RoundToInt(Stats.valueA);
        float spread = Stats.valueB;

        for (int i = 0; i < pellets; i++)
        {
            float a = Random.Range(-spread, spread);
            Vector2 dir = Rotate(baseDir, a);

            GameObject p = Instantiate(pelletPrefab, transform.position,
                                       Quaternion.AngleAxis(
                                           Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.forward));
            p.GetComponent<Rigidbody2D>().linearVelocity = dir * pelletSpeed;

            Bullet b = p.GetComponent<Bullet>();
            if (b != null) { b.SetDamage(TotalDamage); b.SetKnockback(knockback); }
        }
    }
}