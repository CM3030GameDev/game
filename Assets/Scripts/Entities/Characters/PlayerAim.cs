using UnityEngine;

public enum AimMode { Directional, Manual }

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Character character;   // to read movement input
    private Camera cam;

    public AimMode Mode { get; private set; } = AimMode.Directional;
    public Vector2 AimDirection { get; private set; } = Vector2.right;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        // Left click will toggle between directional aim and manual aim
        if (Input.GetMouseButtonDown(0))
            Mode = (Mode == AimMode.Directional) ? AimMode.Manual : AimMode.Directional;

        if (Mode == AimMode.Manual) // Manual aim will follow mouse location
        {
            Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 toMouse = (Vector2)mouseWorld - (Vector2)transform.position;
            if (toMouse.sqrMagnitude > 0.001f)
                AimDirection = toMouse.normalized;
        }
        else // Directional aim follows movement keys (WASD), which will snap to 8 directions
        {
            Vector2 move = character.MoveInput;
            if (move.sqrMagnitude > 0.001f)
                AimDirection = SnapTo8(move);
        }
    }

    // Snap any vector to the nearest of 8 compass directions
    private Vector2 SnapTo8(Vector2 v)
    {
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        float snapped = Mathf.Round(angle / 45f) * 45f;
        float rad = snapped * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }
}