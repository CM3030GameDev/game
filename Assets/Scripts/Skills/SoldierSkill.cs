using UnityEngine;

public class SoldierSkill : MonoBehaviour
{
    [Header("Panic Button")]
    [SerializeField] private int killsToCharge = 10;
    [SerializeField] private int damage = 500;
    [SerializeField] private float hitFlash = 0.1f;

    private int killsSinceLastUse;
    private int lastSeenKillCount;

    public bool IsReady => killsSinceLastUse >= killsToCharge;
    public float ChargeFraction => Mathf.Clamp01((float)killsSinceLastUse / killsToCharge);

    private void Start()
    {
        lastSeenKillCount = MobManager.Instance.GetKillCount();
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        // Track kills via MobManager's running count rather than a per-kill event,
        // so kills caused by this skill's own blast (see Activate) can be excluded.
        int currentKillCount = MobManager.Instance.GetKillCount();
        int delta = currentKillCount - lastSeenKillCount;
        lastSeenKillCount = currentKillCount;
        if (delta > 0)
            killsSinceLastUse = Mathf.Min(killsToCharge, killsSinceLastUse + delta);

        if (Input.GetMouseButtonDown(1) && IsReady)
            Activate();
    }

    private void Activate()
    {
        killsSinceLastUse = 0;

        // TODO: play panic-button animation once art/animation exists

        int willDieCount = 0;
        foreach (var e in MobManager.Instance.GetAllPooledEnemies())
        {
            if (!e.activeInHierarchy) continue;

            Mob mob = e.GetComponent<Mob>();
            if (mob == null) continue;

            if (mob.WouldDie(damage)) willDieCount++;
            mob.GuaranteedAttack(damage, hitFlash);
        }

        // These enemies will die and increment MobManager's kill count once their death
        // animation finishes (a few frames from now) - pre-account for that now so this
        // blast's own kills don't recharge the skill.
        lastSeenKillCount += willDieCount;
    }

    public void ApplyUpgrade(SkillStat stat, float amount)
    {
        switch (stat)
        {
            case SkillStat.Cooldown: killsToCharge = Mathf.Max(1, killsToCharge - (int)amount); break;
            case SkillStat.StrikeCount: damage += (int)amount; break;
        }
    }
}
