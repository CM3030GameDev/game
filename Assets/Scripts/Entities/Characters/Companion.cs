using UnityEngine;

public class Companion : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private Rigidbody2D characterRb;
    private float distance;
    private Vector2 normalizedFollow;

    [SerializeField] private GameObject character;

    [Header("Idling")]
    [Tooltip("With nothing to fight, the companion holds position while within this range of the " +
             "player, and only closes in once it falls further behind.")]
    [SerializeField] private float idleRadius = 3f;

    [Header("Engaging")]
    [Tooltip("Enemies within this range of the PLAYER are engaged, and the companion will not " +
             "stray further than this from the player.")]
    [SerializeField] private float engageRadius = 6f;
    [Tooltip("How close the companion tries to get to its target. Melee wants ~1, a flamethrower " +
             "wants to hang back around 3.")]
    [SerializeField] private float engageDistance = 1.5f;

    [Header("Movement")]
    [Tooltip("Speed used when the player is standing still. While the player moves, the companion " +
             "matches their speed times Catch Up Multiplier instead.")]
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float catchUpMultiplier = 1.1f;
    [Tooltip("World units from the PLAYER. Past this for longer than Leash Time, the companion " +
             "teleports back rather than trying to path around whatever is blocking it.")]
    [SerializeField] private float leashDistance = 10f;
    [SerializeField] private float leashTime = 1.5f;

    private float stuckTimer;
    private Vector2 destination;

    // The enemy being fought. Weapons read this instead of running their own search.
    public Transform Target { get; private set; }

    private static readonly Collider2D[] Hits = new Collider2D[16];
    private static int EnemyMask;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        characterRb = character.GetComponent<Rigidbody2D>();
        EnemyMask = LayerMask.GetMask("Enemy");
    }

    // Searched around the player, not the companion, so engageRadius doubles as the tether.
    private Transform FindTarget()
    {
        Vector2 player = character.transform.position;
        int count = Physics2D.OverlapCircleNonAlloc(player, engageRadius, Hits, EnemyMask);

        Transform nearest = null;
        float nearestSqr = float.MaxValue;
        for (int i = 0; i < count; i++)
        {
            float sqr = ((Vector2)Hits[i].transform.position - (Vector2)transform.position).sqrMagnitude;
            if (sqr < nearestSqr)
            {
                nearestSqr = sqr;
                nearest = Hits[i].transform;
            }
        }
        return nearest;
    }

    // Fighting: a point engageDistance short of the target, clamped inside engageRadius of the
    // player. Idle: stand still unless the player has got further than idleRadius away, which is
    // what lets the companions drift apart naturally instead of steering at a fixed slot.
    private Vector2 Destination()
    {
        Vector2 player = character.transform.position;

        if (Target != null)
        {
            Vector2 enemy = Target.position;
            Vector2 approach = (Vector2)transform.position - enemy;
            Vector2 want = approach.sqrMagnitude > 0.0001f
                ? enemy + approach.normalized * engageDistance
                : enemy;

            Vector2 fromPlayer = want - player;
            if (fromPlayer.magnitude > engageRadius) want = player + fromPlayer.normalized * engageRadius;
            return want;
        }

        Vector2 toPlayer = player - (Vector2)transform.position;
        if (toPlayer.magnitude <= idleRadius) return transform.position;
        return player - toPlayer.normalized * idleRadius;
    }

    // Reads the player's real velocity, so this keeps up after Move Speed upgrades.
    private float FollowSpeed =>
        Mathf.Max(followSpeed, characterRb.linearVelocity.magnitude * catchUpMultiplier);

    private void Update()
    {
        Target = FindTarget();
        destination = Destination();

        Vector2 toDestination = destination - (Vector2)transform.position;
        distance = toDestination.magnitude;
        normalizedFollow = toDestination.normalized;
    }

    private void FixedUpdate()
    {
        // Measured to the player, not the destination, so this reads as "how far has it wandered
        // or been left behind" rather than depending on whatever it is currently chasing.
        Vector2 player = character.transform.position;
        if (Vector2.Distance(transform.position, player) > leashDistance)
        {
            stuckTimer += Time.fixedDeltaTime;
            if (stuckTimer >= leashTime)
            {
                // Offset randomly so two leashed companions don't land on the same pixel.
                rb.position = player + Random.insideUnitCircle.normalized * idleRadius;
                rb.linearVelocity = Vector2.zero;
                stuckTimer = 0f;
                return;
            }
        }
        else stuckTimer = 0f;

        bool moving = distance > 0.2f;
        animator.SetBool("move", moving);

        // Cap the step at the distance left, so it cannot overshoot and bounce back (jitter).
        float step = Mathf.Min(FollowSpeed, distance / Time.fixedDeltaTime);
        rb.linearVelocity = moving ? normalizedFollow * step : Vector2.zero;

        // Face the target, else the way it is walking, else keep the facing it already has.
        float aimX = Target != null ? Target.position.x - transform.position.x : 0f;

        if (Target != null && Mathf.Abs(aimX) > 0.1f) sr.flipX = aimX > 0f;
        else if (moving && Mathf.Abs(normalizedFollow.x) > 0.1f) sr.flipX = normalizedFollow.x > 0f;
    }
}
