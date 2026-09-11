using UnityEngine;

// Gently pulses a SpriteRenderer's alpha, and optionally its scale.
// Scale is off by default because this also sits on objects whose size is driven by another
// script (the EMP field takes its radius from the weapon's level data). Only turn it on for
// objects with a fixed scale, since the base scale is cached once at startup.
[RequireComponent(typeof(SpriteRenderer))]
public class PulseSprite : MonoBehaviour
{
    [SerializeField] private float minAlpha = 0.15f;
    [SerializeField] private float maxAlpha = 0.5f;
    [SerializeField] private float pulsesPerSecond = 1f;

    [Header("Scale (optional)")]
    [Tooltip("Leave off for anything whose scale another script controls.")]
    [SerializeField] private bool pulseScale;
    [SerializeField] private float minScale = 0.9f;
    [SerializeField] private float maxScale = 1.1f;

    private SpriteRenderer sr;
    private Vector3 baseScale;
    private float t;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;
    }

    private void Update()
    {
        t += Time.deltaTime * pulsesPerSecond;

        // Sine remapped to 0..1 so the alpha eases at both ends instead of bouncing linearly
        float wave = (Mathf.Sin(t * Mathf.PI * 2f) + 1f) * 0.5f;

        Color c = sr.color;          // RGB stays whatever you picked in the Inspector
        c.a = Mathf.Lerp(minAlpha, maxAlpha, wave);
        sr.color = c;

        if (pulseScale) transform.localScale = baseScale * Mathf.Lerp(minScale, maxScale, wave);
    }
}
