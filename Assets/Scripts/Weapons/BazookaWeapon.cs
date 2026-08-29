using UnityEngine;

public class BazookaWeapon : SecondaryWeapon
{
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private float rocketSpeed = 10f;

    protected override void Fire()
    {
        GameObject target = FindNearestEnemy();
        if (target == null) return;

        Vector2 dir = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
        GameObject r = Instantiate(rocketPrefab, transform.position,
                                   Quaternion.AngleAxis(
                                       Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, Vector3.forward));
        r.GetComponent<Rigidbody2D>().linearVelocity = dir * rocketSpeed;

        Rocket rk = r.GetComponent<Rocket>();
        if (rk != null) rk.Configure(Stats.damage, Stats.valueA);   // valueA = blast radius
    }
}