using UnityEngine;

public enum AimMode { Auto, Manual }

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private float autoAimRange = 12f;

    private Camera cam;

    // Overlap query rather than MobManager's pool: bosses are placed in the scene, not pooled,
    // so a pool walk cannot see them. Anything on the Enemy layer is a valid target.
    private static readonly Collider2D[] Hits = new Collider2D[32];
    private static int EnemyMask;

    public AimMode Mode { get; private set; } = AimMode.Auto;
    public Vector2 AimDirection { get; private set; } = Vector2.right;

    private void Awake()
    {
        cam = Camera.main;
        EnemyMask = LayerMask.GetMask("Enemy");
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        // Left-click to toggle between auto-aim and manual-aim
        if (Input.GetMouseButtonDown(0))
            Mode = (Mode == AimMode.Auto) ? AimMode.Manual : AimMode.Auto;

        if (Mode == AimMode.Manual)
        {
            Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 toMouse = (Vector2)mouseWorld - (Vector2)transform.position;
            if (toMouse.sqrMagnitude > 0.001f)
                AimDirection = toMouse.normalized;
        }
        else
        {
            Transform target = FindNearestEnemy();
            if (target != null)
            {
                Vector2 toTarget = (Vector2)target.position - (Vector2)transform.position;
                if (toTarget.sqrMagnitude > 0.001f)
                    AimDirection = toTarget.normalized;
            }
            // no enemy in range: keep last aim direction
        }
    }

    private Transform FindNearestEnemy()
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, autoAimRange, Hits, EnemyMask);

        Transform nearest = null;
        float nearestSqr = float.MaxValue;
        for (int i = 0; i < count; i++)
        {
            float sqr = ((Vector2)Hits[i].transform.position - (Vector2)transform.position).sqrMagnitude;
            if (sqr < nearestSqr) { nearestSqr = sqr; nearest = Hits[i].transform; }
        }
        return nearest;
    }
}