using System.Collections.Generic;
using UnityEngine;

// Combat drones that orbit the player and damage anything they touch.
// levels[]: damage, range (orbit radius), valueA (squad size), valueB (lifetime, 0 = forever).
public class DroneWeapon : SecondaryWeapon
{
    [SerializeField] private GameObject dronePrefab;
    [Tooltip("Degrees per second with a single drone.")]
    [SerializeField] private float orbitSpeed = 120f;
    [Tooltip("Extra orbit speed per additional drone, as a fraction of the base. 0.33 means a " +
             "4-drone formation spins at twice the speed of a lone drone.")]
    [SerializeField] private float orbitSpeedPerDrone = 0.33f;
    [Tooltip("Gap between damage ticks, per drone per enemy - each drone can hit the same " +
             "enemy this often, and drones don't block each other.")]
    [SerializeField] private float hitCooldown = 0.5f;

    private readonly List<GameObject> drones = new List<GameObject>();
    private float orbitAngle;
    private float squadTimer;

    public int DroneDamage => TotalDamage;

    private int SquadSize => Mathf.Max(1, Mathf.RoundToInt(Stats.valueA));

    protected override void Update()
    {
        base.Update();   // handles the fireInterval timer -> Fire()

        if (data == null || Time.timeScale == 0f) return;

        // One timer for the whole squad, so every drone appears and vanishes together.
        if (drones.Count > 0 && !IsCombined && Stats.valueB > 0f)
        {
            squadTimer -= Time.deltaTime;
            if (squadTimer <= 0f)
            {
                DespawnSquad();
                timer = 0f;   // restart the gap now, so downtime is exactly one fireInterval
                return;
            }
        }

        OrbitDrones();
    }

    protected override void Fire()
    {
        drones.RemoveAll(d => d == null);

        // Combined drones never expire, so a level up just tops the formation up.
        if (IsCombined)
        {
            while (drones.Count < SquadSize) Spawn();
            return;
        }

        if (drones.Count > 0) return;   // current squad is still flying

        for (int i = 0; i < SquadSize; i++) Spawn();
        squadTimer = Stats.valueB;
    }

    private void Spawn()
    {
        GameObject d = Instantiate(dronePrefab, transform.position, Quaternion.identity, transform);
        d.GetComponent<CombatDrone>()?.Configure(this, hitCooldown);
        drones.Add(d);
    }

    private void DespawnSquad()
    {
        foreach (GameObject d in drones)
            if (d != null) Destroy(d);
        drones.Clear();
    }

    private void OrbitDrones()
    {
        drones.RemoveAll(d => d == null);
        if (drones.Count == 0) return;

        // Bigger squads spin faster. Uses squad size, since combining resets level back to 1.
        float speed = orbitSpeed * (1f + (drones.Count - 1) * orbitSpeedPerDrone);

        orbitAngle += speed * Time.deltaTime;
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
