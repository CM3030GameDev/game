using System.Collections;
using UnityEngine;

// Mercenary Smokescreen. Slows every enemy on screen for the duration and deals no damage.
public class FireSkill : PlayerSkill
{
    [Header("Smokescreen")]
    [SerializeField] private GameObject fireOverlay;      // the top/bottom fire strips, under the HUD
    [SerializeField] private float duration = 8f;
    [SerializeField] private float tickInterval = 0.4f;
    [SerializeField] private float slowMultiplier = 0.4f;

    private Camera cam;
    private static int EnemyMask;

    protected override void Start()
    {
        base.Start();
        cam = Camera.main;
        EnemyMask = LayerMask.GetMask("Enemy");
        if (fireOverlay != null) fireOverlay.SetActive(false);
    }

    protected override IEnumerator Cast()
    {
        if (fireOverlay != null) fireOverlay.SetActive(true);

        try
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                SlowEnemiesOnScreen();
                yield return new WaitForSeconds(tickInterval);
                elapsed += tickInterval;
            }
        }
        finally
        {
            if (fireOverlay != null) fireOverlay.SetActive(false);
        }
    }

    // Re-applied each tick, since the camera moves and enemies can walk in partway through.
    private void SlowEnemiesOnScreen()
    {
        if (cam == null) return;

        float height = cam.orthographicSize * 2f;
        Vector2 size = new Vector2(height * cam.aspect, height);

        // OverlapBoxAll, since a full screen can hold more enemies than a fixed buffer.
        Collider2D[] hits = Physics2D.OverlapBoxAll(cam.transform.position, size, 0f, EnemyMask);
        foreach (var h in hits)
        {
            Mob mob = h.GetComponent<Mob>();
            if (mob != null) mob.ApplyDebuff(slowMultiplier, tickInterval * 1.5f);
        }
    }
}
