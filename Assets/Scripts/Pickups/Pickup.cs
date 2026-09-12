using UnityEngine;

public enum PickupType { Health, Magnet }

// A thing lying on the ground that does something once when the player walks over it.
// Same collection pattern as ExpOrb (distance check, not a trigger) so both behave identically.
public class Pickup : MonoBehaviour
{
    [SerializeField] private PickupType type;
    [SerializeField] private CharacterStats characterStats;
    [Tooltip("Health only. Healing is capped at max health.")]
    [SerializeField] private int healAmount = 25;
    [SerializeField] private float collectDistance = 0.8f;
    [Tooltip("Index into UIAudioManager's Sound Effects list. Set per prefab. -1 plays nothing.")]
    [SerializeField] private int collectSfx = -1;
    [Range(0f, 1f)][SerializeField] private float collectSfxVolume = 0.8f;
    [SerializeField] private float moveSpeed = 12f;

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

        // Same magnet behaviour as ExpOrb, so Pickup Range upgrades pull every collectable in.
        // ponytail: third copy of this movement block. Extract a shared Collectable base if a
        // fourth collectable type appears - two copies was not worth a base class, four is.
        if (magnetised || dist <= characterStats.pickupRadius)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, player.position, moveSpeed * Time.deltaTime);
        }

        if (dist > collectDistance) return;

        Collect();
        UIAudioManager.Sfx(collectSfx, collectSfxVolume);
        Destroy(gameObject);
    }

    // Called by a Magnet pickup. Ignores pickupRadius from here on.
    public void Magnetise() => magnetised = true;

    private void Collect()
    {
        switch (type)
        {
            case PickupType.Health:
                characterStats.health = Mathf.Min(characterStats.maxHealth,
                                                  characterStats.health + healAmount);
                break;

            case PickupType.Magnet:
                // Flips things into magnetised mode rather than moving them from here, so each
                // collectable keeps owning its own movement and collection.
                foreach (ExpOrb orb in FindObjectsByType<ExpOrb>(FindObjectsSortMode.None))
                    orb.Magnetise();

                // Other magnets are skipped: chaining them would clear the map in one pickup.
                foreach (Pickup other in FindObjectsByType<Pickup>(FindObjectsSortMode.None))
                    if (other != this && other.type != PickupType.Magnet) other.Magnetise();
                break;
        }
    }
}
