using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private int expValue = 10;        // Flat Value (change later!!)
    [SerializeField] private float moveSpeed = 12f;      // Move speed of the exp orb when the player is in range
    [SerializeField] private float collectDistance = 0.3f;
    [Tooltip("Index into UIAudioManager's Sound Effects list. -1 plays nothing.")]
    [SerializeField] private int collectSfx = -1;
    [Range(0f, 1f)][SerializeField] private float collectSfxVolume = 0.4f;
    [Tooltip("Top speed a magnetised orb reaches. Orbs far from the player travel at this, " +
             "easing back to Move Speed as they arrive.")]
    [SerializeField] private float magnetSpeed = 20f;
    [Tooltip("Distance at which a magnetised orb is already at full Magnet Speed.")]
    [SerializeField] private float magnetRampDistance = 15f;

    private Transform player;
    private bool magnetised;

    private void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Character");
        if (p != null) player = p.transform;
    }

    private void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        // Magnet effect within the player's pickup radius, or from anywhere once magnetised
        if (magnetised || dist <= characterStats.pickupRadius)
        {
            // Magnetised orbs scale their speed with distance, so ones across the map come in
            // quickly instead of crawling, then ease back to normal speed as they arrive.
            float speed = magnetised
                ? Mathf.Lerp(moveSpeed, magnetSpeed, Mathf.Clamp01(dist / magnetRampDistance))
                : moveSpeed;

            transform.position = Vector2.MoveTowards(
                transform.position, player.position, speed * Time.deltaTime);
        }

        // Collects when close enough and deletes the exp orb once collected
        if (dist <= collectDistance)
        {
            characterStats.expPoint += expValue;
            UIAudioManager.Sfx(collectSfx, collectSfxVolume);
            Destroy(gameObject);
        }
    }

    // Called by a Magnet pickup. Ignores pickupRadius from here on, so the orb crosses the
    // whole map instead of waiting for the player to walk near it.
    public void Magnetise() => magnetised = true;
}