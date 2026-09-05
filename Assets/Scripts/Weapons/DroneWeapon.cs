using System.Collections.Generic;
using UnityEngine;

// Combat drones - orbit the player and chip at anything they touch.
// levels[]: damage = per-hit damage, range = orbit radius, valueA = max drones,
//           valueB = drone lifetime in seconds, fireInterval = respawn cadence.
// Combined form ignores valueB entirely, so its drones never expire.
public class DroneWeapon : SecondaryWeapon
{
    [SerializeField] private GameObject dronePrefab;
    [SerializeField] private float orbitSpeed = 90f;   // degrees per second
    [SerializeField] private float hitCooldown = 2f;   // per-drone gap between damage ticks

    private readonly List<GameObject> drones = new List<GameObject>();
    private float orbitAngle;

    public int DroneDamage => TotalDamage;

    protected override void Update()
    {
        base.Update();   // handles the fireInterval timer -> Fire()

        if (data == null || Time.timeScale == 0f) return;
        OrbitDrones();
    }

    protected override void Fire()
    {
        drones.RemoveAll(d => d == null);   // clear expired drones

        int maxDrones = Mathf.RoundToInt(Stats.valueA);
        if (drones.Count >= maxDrones) return;

        GameObject d = Instantiate(dronePrefab, transform.position, Quaternion.identity, transform);

        CombatDrone drone = d.GetComponent<CombatDrone>();
        if (drone != null)
            drone.Configure(this, hitCooldown, IsCombined ? 0f : Stats.valueB);

        drones.Add(d);
    }

    private void OrbitDrones()
    {
        drones.RemoveAll(d => d == null);
        if (drones.Count == 0) return;

        orbitAngle += orbitSpeed * Time.deltaTime;
        if (orbitAngle >= 360f) orbitAngle -= 360f;

        // Spread evenly around the player so the formation stays balanced as drones come and go
        float step = 360f / drones.Count;

        for (int i = 0; i < drones.Count; i++)
        {
            float a = (orbitAngle + step * i) * Mathf.Deg2Rad;
            drones[i].transform.localPosition =
                new Vector3(Mathf.Cos(a), Mathf.Sin(a), 0f) * Stats.range;
        }
    }
}
