using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Act1Boss : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHP = 300;
    [SerializeField] private Image healthBarFill; // Image Type: Filled, Horizontal, Origin Left
    [Header("Audio")]
    [Tooltip("Index into UIAudioManager's Sound Effects list. -1 plays nothing.")]
    [SerializeField] private int dashSfx = -1;
    [SerializeField] private int slamSfx = -1;
    [SerializeField] private int deathSfx = -1;

    [Header("Companion damage")]
    [SerializeField] private int swordDamage = 20;
    [SerializeField] private float swordCooldown = 0.5f;
    [SerializeField] private int flamethrowerDamage = 5;
    [SerializeField] private float flamethrowerCooldown = 0.25f;

    [SerializeField] private Color hitFlashColor = Color.red;
    private int currentHP;
    private float hitFlashTimer;
    private float companionHitTimer;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private LayerMask characterLayer;

    [Header("Contact Damage")]
    [SerializeField] private int contactDamage = 15;
    [SerializeField] private float contactInvulnerability = 0.5f;

    [Header("Attack Cooldown")]
    [SerializeField] private float attackCooldown = 5f;
    private float cooldownTimer;
    private bool isAttacking;

    [Header("Dash Attack (used when the player is far)")]
    [SerializeField] private float dashRange = 6f;
    [SerializeField] private float dashTelegraphDuration = 0.8f;
    [SerializeField] private float dashDuration = 0.4f;
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private Transform dashLineIndicator; // rotated to face the dash direction while telegraphing

    [Header("AoE Slam (used when the player is close)")]
    [SerializeField] private float aoeRadius = 3f;
    [SerializeField] private float aoeTelegraphDuration = 0.8f;
    [SerializeField] private int aoeDamage = 15;
    [SerializeField] private SpriteRenderer aoeIndicator; // circle sprite, fades in/out while telegraphing

    [Header("Shake")]
    [SerializeField] private float shakeIntensity = 0.1f;

    public UnityEvent bossDeath;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Transform player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectsWithTag("Character")[0].transform;
    }

    private void OnEnable()
    {
        currentHP = maxHP;
        cooldownTimer = attackCooldown; // give the player a beat before the first attack
        isAttacking = false;
        if (sr != null) sr.color = Color.white;
        UpdateHealthBar();
    }

    private void Update()
    {
        HandleHitFlash();
        if (companionHitTimer > 0f) companionHitTimer -= Time.deltaTime;

        if (currentHP <= 0) { rb.linearVelocity = Vector2.zero; return; }

        if (sr != null) sr.flipX = player.position.x < transform.position.x;

        if (isAttacking) return;

        rb.linearVelocity = ((Vector2)player.position - (Vector2)transform.position).normalized * moveSpeed;

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist > dashRange)
            StartCoroutine(DashAttackRoutine());
        else if (dist <= aoeRadius)
            StartCoroutine(AoEAttackRoutine());
        // between the two ranges: keep chasing, don't burn the cooldown slamming at nothing
    }

    // Every scene owns its own UIAudioManager, so this is null whenever a scene has none.
    private static void PlaySfx(int index)
    {
        if (index >= 0 && UIAudioManager.Instance != null) UIAudioManager.Instance.PlaySFXOneShot(index);
    }

    private IEnumerator DashAttackRoutine()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;
        PlaySfx(dashSfx);

        Vector2 dashDir = ((Vector2)player.position - (Vector2)transform.position).normalized;

        if (dashLineIndicator != null)
        {
            dashLineIndicator.gameObject.SetActive(true);
            float angle = Mathf.Atan2(dashDir.y, dashDir.x) * Mathf.Rad2Deg;
            dashLineIndicator.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        yield return Shake(dashTelegraphDuration);

        if (dashLineIndicator != null) dashLineIndicator.gameObject.SetActive(false);

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            rb.linearVelocity = dashDir * dashSpeed;
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        cooldownTimer = attackCooldown;
        isAttacking = false;
    }

    private IEnumerator AoEAttackRoutine()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;
        PlaySfx(slamSfx);

        if (aoeIndicator != null)
        {
            aoeIndicator.gameObject.SetActive(true);
            aoeIndicator.transform.localScale = Vector3.one * (aoeRadius * 2f);
        }

        Vector3 basePos = transform.position;
        float t = 0f;
        while (t < aoeTelegraphDuration)
        {
            t += Time.deltaTime;

            if (aoeIndicator != null)
            {
                float pulse = (Mathf.Sin(t * 10f) + 1f) * 0.5f;
                Color c = aoeIndicator.color;
                c.a = Mathf.Lerp(0.2f, 0.8f, pulse);
                aoeIndicator.color = c;
            }

            transform.position = basePos + (Vector3)(Random.insideUnitCircle * shakeIntensity);
            yield return null;
        }
        transform.position = basePos;

        if (aoeIndicator != null) aoeIndicator.gameObject.SetActive(false);

        Collider2D hit = Physics2D.OverlapCircle(transform.position, aoeRadius, characterLayer);
        if (hit != null)
        {
            Character c = hit.GetComponent<Character>();
            if (c != null)
            {
                c.CharacterAttacked(aoeDamage, 0.3f);
            }
        }

        cooldownTimer = attackCooldown;
        isAttacking = false;
    }

    private IEnumerator Shake(float duration)
    {
        Vector3 basePos = transform.position;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.position = basePos + (Vector3)(Random.insideUnitCircle * shakeIntensity);
            yield return null;
        }
        transform.position = basePos;
    }

    // Companion weapons reach the boss the same way they reach a Mob, on their own cooldown.
    // BossAttacked has no rate limit of its own, so without this the flame would tick every frame.
    private void OnTriggerStay2D(Collider2D other)
    {
        if (currentHP <= 0) return;

        // Contact damage is Stay, not Enter, because the boss collider is a trigger so the player
        // can walk through it. The player's own i-frames rate limit this.
        if (other.CompareTag("Character"))
        {
            other.GetComponent<Character>()?.CharacterAttacked(contactDamage, contactInvulnerability);
            return;
        }

        if (companionHitTimer > 0f) return;

        if (other.CompareTag("Sword"))
        {
            companionHitTimer = swordCooldown;
            BossAttacked(swordDamage, 0.1f);
        }
        else if (other.CompareTag("Flamethrower"))
        {
            companionHitTimer = flamethrowerCooldown;
            BossAttacked(flamethrowerDamage, 0.1f);
        }
    }

    public void BossAttacked(int amount, float flashDuration)
    {
        if (currentHP <= 0) return;

        currentHP -= amount;
        hitFlashTimer = flashDuration;
        UpdateHealthBar();

        if (currentHP <= 0) Die();
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = Mathf.Clamp01((float)currentHP / maxHP);
    }

    private void HandleHitFlash()
    {
        if (sr == null || hitFlashTimer <= 0f) return;

        hitFlashTimer -= Time.deltaTime;
        sr.color = hitFlashTimer > 0f ? hitFlashColor : Color.white;
    }

    private void Die()
    {
        StopAllCoroutines();
        PlaySfx(deathSfx);
        bossDeath?.Invoke();
        gameObject.SetActive(false);
    }
}
