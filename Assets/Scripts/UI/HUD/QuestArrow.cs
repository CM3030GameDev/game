using UnityEngine;

// Rotates to point from the player toward the current objective. Hides itself when there's
// no single point to point at (e.g. "kill enemies here" / "defeat the miniboss" objectives).
public class QuestArrow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float angleOffset = 0f; // adjust to match whichever way the arrow sprite points by default
    [SerializeField] private float orbitRadius = 1f; // how far from the player the arrow floats
    private Transform target;

    private void Awake()
    {
        gameObject.SetActive(false); // hidden until something actually calls SetTarget
    }

    public void SetTarget(Transform t)
    {
        target = t;
        // Set active state here directly rather than relying on Update() to notice -
        // a disabled GameObject's Update() never runs, so it could never turn itself back on.
        gameObject.SetActive(target != null);
    }

    private void Update()
    {
        if (target == null) return;

        Vector2 dir = (Vector2)target.position - (Vector2)player.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.position = (Vector2)player.position + dir.normalized * orbitRadius;
        transform.rotation = Quaternion.Euler(0, 0, angle + angleOffset);
    }
}
