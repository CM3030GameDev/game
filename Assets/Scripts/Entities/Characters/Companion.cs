using UnityEngine;

public class Companion : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private Rigidbody2D characterRb;
    private SpriteRenderer characterSr;
    private float distance;
    private Vector2 normalizedFollow;

    [SerializeField] private GameObject character;
    [Tooltip("Where the companion idles relative to the player when there's nothing to fight.")]
    [SerializeField] private float followRange = 2f;
    [Tooltip("Degrees around the player this companion idles at. Give each companion a different " +
             "angle (e.g. 0 and 180) so they flank instead of stacking on the same spot.")]
    [SerializeField] private float followAngle;

    [Header("Engaging")]
    [Tooltip("Enemies within this range of the PLAYER are engaged, and the companion will not " +
             "stray further than this from the player - it doubles as the tether.")]
    [SerializeField] private float engageRadius = 6f;
    [Tooltip("How close the companion tries to get to its target. Melee wants ~1, a flamethrower " +
             "wants to hang back around 3.")]
    [SerializeField] private float engageDistance = 1.5f;

    [Header("Movement")]
    [Tooltip("Speed used when the player is standing still. While the player moves, the companion " +
             "matches their speed times Catch Up Multiplier instead, so Move Speed upgrades can't " +
             "leave it behind.")]
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float catchUpMultiplier = 1.1f;
    [Tooltip("If the companion is stuck this far from where it wants to be for longer than Leash " +
             "Time - held up on a wall corner, or left behind through a doorway - it teleports.")]
    [SerializeField] private float leashDistance = 12f;
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
        characterSr = character.GetComponent<SpriteRenderer>();
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

    // Where it wants to stand: its idle slot, or just short of its target when fighting.
    private Vector2 Destination()
    {
        Vector2 player = character.transform.position;

        if (Target == null)
        {
            Vector2 offset = new Vector2(Mathf.Cos(followAngle * Mathf.Deg2Rad),
                                         Mathf.Sin(followAngle * Mathf.Deg2Rad));
            return player + offset * followRange;
        }

        Vector2 enemy = Target.position;
        Vector2 approach = (Vector2)transform.position - enemy;
        Vector2 want = approach.sqrMagnitude > 0.0001f
            ? enemy + approach.normalized * engageDistance
            : enemy;

        Vector2 fromPlayer = want - player;
        if (fromPlayer.magnitude > engageRadius) want = player + fromPlayer.normalized * engageRadius;
        return want;
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
        // Teleport instead of pathfinding when a wall has kept it stuck for long enough.
        if (distance > leashDistance)
        {
            stuckTimer += Time.fixedDeltaTime;
            if (stuckTimer >= leashTime)
            {
                rb.position = destination;
                rb.linearVelocity = Vector2.zero;
                stuckTimer = 0f;
                return;
            }
        }
        else stuckTimer = 0f;

        bool moving = distance > followRange * 0.15f;
        animator.SetBool("move", moving);

        // Cap the step at the distance left, so it cannot overshoot and bounce back (jitter).
        float step = Mathf.Min(FollowSpeed, distance / Time.fixedDeltaTime);
        rb.linearVelocity = moving ? normalizedFollow * step : Vector2.zero;

        // Face the target first, then the way it is walking, then whatever the player faces.
        float aimX = Target != null ? Target.position.x - transform.position.x : 0f;

        if (Target != null && Mathf.Abs(aimX) > 0.1f) sr.flipX = aimX > 0f;
        else if (moving && Mathf.Abs(normalizedFollow.x) > 0.1f) sr.flipX = normalizedFollow.x > 0f;
        else if (!moving && Target == null && characterSr != null) sr.flipX = characterSr.flipX;
    }
}
