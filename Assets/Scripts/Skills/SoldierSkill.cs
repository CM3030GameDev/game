using System.Collections;
using UnityEngine;

// Soldier Air Strike. Hits every enemy on the field at once, with staggered explosions.
public class SoldierSkill : PlayerSkill
{
    [Header("Air Strike")]
    [SerializeField] private int damage = 500;
    [SerializeField] private float hitFlash = 0.1f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float maxExplosionDelay = 0.3f;

    protected override IEnumerator Cast()
    {
        int willDieCount = 0;

        foreach (var e in MobManager.Instance.GetAllPooledEnemies())
        {
            if (!e.activeInHierarchy) continue;

            Mob mob = e.GetComponent<Mob>();
            if (mob == null) continue;

            if (mob.WouldDie(damage)) willDieCount++;
            mob.GuaranteedAttack(damage, hitFlash);

            if (explosionPrefab != null)
                StartCoroutine(Kaboom(e.transform.position, Random.Range(0f, maxExplosionDelay)));
        }

        // These enemies die once their death animation finishes, a few frames from now.
        // Pre-account for it so this blast's own kills don't recharge the skill.
        IgnoreKills(willDieCount);

        yield break;
    }

    private IEnumerator Kaboom(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(explosionPrefab, position, Quaternion.identity);
    }

    public void ApplyUpgrade(SkillStat stat, float amount)
    {
        switch (stat)
        {
            case SkillStat.Cooldown: cooldown = Mathf.Max(1f, cooldown - amount); break;
            case SkillStat.StrikeCount: damage += (int)amount; break;
        }
    }
}
