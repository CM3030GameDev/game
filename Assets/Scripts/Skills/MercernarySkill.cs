using UnityEngine;
using System.Collections.Generic;

public class MercernarySkill : MonoBehaviour
{
    [SerializeField] private float duration = 6f;
    [SerializeField] private float radius = 3f;
    [SerializeField] private int tickDamage = 5;
    [SerializeField] private float tickInterval = 0.5f;
    [SerializeField] private float slowMultiplier = 0.5f; // Slow enemies walking in skill area

    private float tickTimer;

    private static int EnemyMask;

    private void Start()
    {
        EnemyMask = LayerMask.GetMask("Enemy");
        Destroy(gameObject, duration);
    }

    private void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer < tickInterval) return;
        tickTimer = 0f;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, EnemyMask);
        foreach (var h in hits)
        {
            Mob mob = h.GetComponent<Mob>();
            if (mob != null)
            {
                mob.MobAttacked(tickDamage, 0.1f);
                mob.ApplyDebuff(slowMultiplier, tickInterval * 1.5f);
            }
        }
    }
}