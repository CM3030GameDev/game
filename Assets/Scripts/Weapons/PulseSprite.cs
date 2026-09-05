using UnityEngine;

// Gently pulses a SpriteRenderer's alpha. Deliberately only touches alpha - never scale -
// so it can sit on an object whose size is driven by another script (e.g. the EMP field,
// whose radius comes from the weapon's level data).
[RequireComponent(typeof(SpriteRenderer))]
public class PulseSprite : MonoBehaviour
{
    [SerializeField] private float minAlpha = 0.15f;
    [SerializeField] private float maxAlpha = 0.5f;
    [SerializeField] private float pulsesPerSecond = 1f;

    private SpriteRenderer sr;
    private float t;

    private void Awake() => sr = GetComponent<SpriteRenderer>();

    private void Update()
    {
        t += Time.deltaTime * pulsesPerSecond;

        // Sine remapped to 0..1 so the alpha eases at both ends instead of bouncing linearly
        float wave = (Mathf.Sin(t * Mathf.PI * 2f) + 1f) * 0.5f;

        Color c = sr.color;          // RGB stays whatever you picked in the Inspector
        c.a = Mathf.Lerp(minAlpha, maxAlpha, wave);
        sr.color = c;
    }
}
