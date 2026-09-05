using System.Collections;
using UnityEngine;

// Mercenary - Fire Line. Utility: sets the top and bottom edges of the screen alight, so
// everything walking in from off-screen gets burned and slowed on the way. Mobs spawn well
// outside the view and path inward, which is what makes the screen edges the right place
// to put a gate rather than a zone around the player.
public class FireSkill : PlayerSkill
{
    [Header("Fire Line")]
    [SerializeField] private GameObject fireOverlay;      // the top/bottom fire strips, under the HUD
    [SerializeField] private float duration = 8f;
    [SerializeField] private float bandHeight = 2.5f;     // world-space thickness of each burning edge
    [SerializeField] private int tickDamage = 8;
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
                BurnScreenEdges();
                yield return new WaitForSeconds(tickInterval);
                elapsed += tickInterval;
            }
        }
        finally
        {
            if (fireOverlay != null) fireOverlay.SetActive(false);
        }
    }

    // Recomputed each tick rather than cached, since the camera follows the player.
    private void BurnScreenEdges()
    {
        if (cam == null) return;

        float halfHeight = cam.orthographicSize;
        float width = halfHeight * cam.aspect * 2f;
        Vector2 centre = cam.transform.position;

        Burn(new Vector2(centre.x, centre.y + halfHeight - bandHeight * 0.5f), width);
        Burn(new Vector2(centre.x, centre.y - halfHeight + bandHeight * 0.5f), width);
    }

    private void Burn(Vector2 centre, float width)
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(centre, new Vector2(width, bandHeight), 0f, EnemyMask);
        foreach (var h in hits)
        {
            Mob mob = h.GetComponent<Mob>();
            if (mob == null) continue;

            mob.MobAttacked(tickDamage, 0.1f);
            mob.ApplyDebuff(slowMultiplier, tickInterval * 1.5f);
        }
    }
}
