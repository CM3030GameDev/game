using System.Collections;
using UnityEngine;

public abstract class PlayerSkill : MonoBehaviour
{
    [Header("Activation")]
    [SerializeField] private KeyCode activationKey = KeyCode.Alpha1;
    [SerializeField] private bool useRightMouse;
    [Tooltip("Seconds before the skill is available again after use.")]
    [SerializeField] protected float cooldown = 30f;
    [Tooltip("Seconds knocked off the remaining cooldown per enemy killed. Kills shorten the " +
             "wait; they don't replace it, so the cooldown is a floor rather than a kill quota.")]
    [SerializeField] private float secondsPerKill = 1f;

    [Header("Screen Effect")]
    [SerializeField] private GameObject screenEffect;
    [SerializeField] private float screenEffectDuration = 0.5f;

    [Header("Companion")]
    [SerializeField] private Transform companion;
    [SerializeField] private float castTime = 0.4f;

    [Header("HUD")]
    [SerializeField] private SkillCooldownUI hud;

    private float cooldownRemaining;
    private int lastSeenKillCount;

    public bool IsReady => cooldownRemaining <= 0f && IsAvailable;

    // Overridden where a skill depends on something else being present, e.g. a companion.
    protected virtual bool IsAvailable => true;
    // Inverted so the HUD radial still fills up towards ready rather than draining.
    public float ChargeFraction => cooldown <= 0f ? 1f : Mathf.Clamp01(1f - cooldownRemaining / cooldown);

    protected virtual void Start()
    {
        lastSeenKillCount = KillCount;
        // Start on cooldown, so the first use still has to be earned.
        cooldownRemaining = cooldown;
        if (screenEffect != null) screenEffect.SetActive(false);
    }

    protected virtual void Update()
    {
        if (Time.timeScale == 0f) return;

        TickCooldown();

        // The slot is hidden entirely while the skill is unavailable, so a scene without the
        // companion shows two slots rather than a dead third one.
        if (hud != null)
        {
            if (hud.gameObject.activeSelf != IsAvailable) hud.gameObject.SetActive(IsAvailable);
            if (IsAvailable) hud.SetCharge(ChargeFraction, IsReady);
        }

        if (!IsReady) return;
        if (!PressedActivate()) return;

        cooldownRemaining = cooldown;
        StartCoroutine(RunCast());
    }

    private bool PressedActivate()
        => useRightMouse ? Input.GetMouseButtonDown(1) : Input.GetKeyDown(activationKey);

    // Time and kills pay down the same counter, so quiet and busy stretches both recharge it.
    private void TickCooldown()
    {
        // Drain the counter every frame, or banked kills would refund the next cooldown at once.
        int current = KillCount;
        int delta = current - lastSeenKillCount;
        lastSeenKillCount = current;

        if (cooldownRemaining <= 0f) return;

        cooldownRemaining -= Time.deltaTime;
        if (delta > 0) cooldownRemaining -= delta * secondsPerKill;
        if (cooldownRemaining < 0f) cooldownRemaining = 0f;
    }

    // Guarded so a scene without a MobManager leaves the skill on a plain timer instead of
    // throwing every frame, which would stop the HUD updating at all.
    private static int KillCount => MobManager.Instance != null ? MobManager.Instance.GetKillCount() : 0;

    protected void IgnoreKills(int count) => lastSeenKillCount += count;

    // Skills can overlap on purpose. Spending the charge up front stops one retriggering itself.
    private IEnumerator RunCast()
    {
        if (companion != null)
        {
            companion.position = transform.position;
            yield return new WaitForSeconds(castTime);
        }

        if (screenEffect != null)
        {
            screenEffect.SetActive(true);
            StartCoroutine(HideScreenEffect());
        }

        yield return Cast();
    }

    private IEnumerator HideScreenEffect()
    {
        yield return new WaitForSeconds(screenEffectDuration);
        if (screenEffect != null) screenEffect.SetActive(false);
    }

    protected abstract IEnumerator Cast();
}
