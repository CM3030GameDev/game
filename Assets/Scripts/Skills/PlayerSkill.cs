using System.Collections;
using UnityEngine;

public abstract class PlayerSkill : MonoBehaviour
{
    [Header("Activation")]
    [SerializeField] private KeyCode activationKey = KeyCode.Alpha1;
    [SerializeField] private bool useRightMouse;
    [SerializeField] protected int killsToCharge = 30;

    [Header("Screen Effect")]
    [SerializeField] private GameObject screenEffect;
    [SerializeField] private float screenEffectDuration = 0.5f;

    [Header("Companion")]
    [SerializeField] private Transform companion;
    [SerializeField] private float castTime = 0.4f;

    [Header("HUD")]
    [SerializeField] private SkillCooldownUI hud;

    private int killsSinceLastUse;
    private int lastSeenKillCount;

    private static bool anyoneCasting;

    public bool IsReady => killsSinceLastUse >= killsToCharge;
    public float ChargeFraction => Mathf.Clamp01((float)killsSinceLastUse / killsToCharge);

    protected virtual void Start()
    {
        lastSeenKillCount = MobManager.Instance.GetKillCount();
        anyoneCasting = false;
        if (screenEffect != null) screenEffect.SetActive(false);
    }

    protected virtual void Update()
    {
        if (Time.timeScale == 0f) return;

        TrackKills();

        if (hud != null) hud.SetCharge(ChargeFraction, IsReady);

        if (anyoneCasting || !IsReady) return;
        if (!PressedActivate()) return;

        killsSinceLastUse = 0;
        StartCoroutine(RunCast());
    }

    private bool PressedActivate()
        => useRightMouse ? Input.GetMouseButtonDown(1) : Input.GetKeyDown(activationKey);

    private void TrackKills()
    {
        int current = MobManager.Instance.GetKillCount();
        int delta = current - lastSeenKillCount;
        lastSeenKillCount = current;

        if (delta > 0)
            killsSinceLastUse = Mathf.Min(killsToCharge, killsSinceLastUse + delta);
    }

    protected void IgnoreKills(int count) => lastSeenKillCount += count;

    private IEnumerator RunCast()
    {
        anyoneCasting = true;
        try
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
        finally
        {
            anyoneCasting = false;
        }
    }

    private IEnumerator HideScreenEffect()
    {
        yield return new WaitForSeconds(screenEffectDuration);
        if (screenEffect != null) screenEffect.SetActive(false);
    }

    protected abstract IEnumerator Cast();
}
