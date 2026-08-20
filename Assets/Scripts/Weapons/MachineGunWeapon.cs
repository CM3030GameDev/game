using UnityEngine;

public class MachineGunWeapon : SecondaryWeapon
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 18f;

    private float angle;

    protected override void Update()
    {
        if (Time.timeScale == 0f) return;

        angle += Stats.valueB * Time.deltaTime;   // valueB = rotation speed
        if (angle >= 360f) angle -= 360f;

        timer += Time.deltaTime;
        if (timer >= Stats.fireInterval) { Fire(); timer = 0f; }
    }

    protected override void Fire()
    {
        int streams = Mathf.RoundToInt(Stats.valueA);
        float step = 360f / streams;

        for (int i = 0; i < streams; i++)
        {
            float a = angle + (step * i);
            Vector2 dir = new Vector2(Mathf.Cos(a * Mathf.Deg2Rad), Mathf.Sin(a * Mathf.Deg2Rad));

            GameObject b = Instantiate(bulletPrefab, transform.position,
                                       Quaternion.AngleAxis(a, Vector3.forward));
            b.GetComponent<Rigidbody2D>().linearVelocity = dir * bulletSpeed;
            Bullet bl = b.GetComponent<Bullet>();
            if (bl != null) bl.SetDamage(Stats.damage);
        }
    }
}