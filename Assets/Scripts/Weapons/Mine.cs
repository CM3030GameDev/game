using System.Collections;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [Tooltip("Index into UIAudioManager's Sound Effects list, played on each explosion. -1 plays nothing.")]
    [SerializeField] private int explodeSfx = -1;
    [Range(0f, 1f)][SerializeField] private float explodeSfxVolume = 0.6f;

    private int damage = 50;
    private float blastRadius = 1.5f;
    private bool detonatesTwice;
    private float secondBlastDelay = 3f;
    private bool hasBlastedOnce;

    private static int EnemyLayer;

    public void Configure(int dmg, float radius, bool twice = false, float delay = 3f)
    {
        damage = dmg;
        blastRadius = radius;
        detonatesTwice = twice;
        secondBlastDelay = delay;
    }

    private void Awake()
    {
        EnemyLayer = LayerMask.NameToLayer("Enemy");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Final boss phases are not on the Enemy layer, so check for a boss as well.
        if (other.gameObject.layer != EnemyLayer && BossDamage.Find(other) == null) return;
        Detonate();
    }

    private void Detonate()
    {
        UIAudioManager.Sfx(explodeSfx, explodeSfxVolume);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, blastRadius, BossDamage.Mask);
        var bossesHit = new System.Collections.Generic.HashSet<MonoBehaviour>();   // one hit per boss, even with several colliders
        foreach (var h in hits)
        {
            Mob mob = h.GetComponent<Mob>();
            if (mob != null) { mob.MobAttacked(damage, 0.1f); continue; }

            MonoBehaviour boss = BossDamage.Find(h);
            if (boss != null && bossesHit.Add(boss)) BossDamage.Damage(boss, damage, 0.1f);
        }

        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // Combined mine: stays on the field and goes off once more after a delay, then it's spent
        if (detonatesTwice && !hasBlastedOnce)
        {
            hasBlastedOnce = true;
            StartCoroutine(SecondBlast());
            return;
        }

        Destroy(gameObject);
    }

    private IEnumerator SecondBlast()
    {
        // Collider off so a mob standing on it can't re-trigger during the recharge, but the
        // sprite stays visible so the player can see the mine is still live.
        SetCollidersEnabled(false);
        yield return new WaitForSeconds(secondBlastDelay);
        Detonate();
    }

    private void SetCollidersEnabled(bool value)
    {
        foreach (var c in GetComponents<Collider2D>()) c.enabled = value;
    }
}
