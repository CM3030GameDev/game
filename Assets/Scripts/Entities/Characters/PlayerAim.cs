using UnityEngine;

public enum AimMode { Auto, Manual }

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private float autoAimRange = 12f;

    private Camera cam;

    public AimMode Mode { get; private set; } = AimMode.Auto;
    public Vector2 AimDirection { get; private set; } = Vector2.right;

    private void Awake()
    {
        cam = Camera.main;
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
            GameObject target = FindNearestEnemy();
            if (target != null)
            {
                Vector2 toTarget = (Vector2)target.transform.position - (Vector2)transform.position;
                if (toTarget.sqrMagnitude > 0.001f)
                    AimDirection = toTarget.normalized;
            }
            // no enemy in range: keep last aim direction
        }
    }

    private GameObject FindNearestEnemy()
    {
        GameObject nearest = null;
        float nearestDist = autoAimRange;

        foreach (GameObject e in MobManager.Instance.GetAllPooledEnemies())
        {
            if (!e.activeInHierarchy) continue;
            float d = Vector2.Distance(transform.position, e.transform.position);
            if (d < nearestDist) { nearestDist = d; nearest = e; }
        }
        return nearest;
    }
}