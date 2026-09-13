using System.Collections;
using UnityEngine;

// Swordsman Guard. A speed burst plus invulnerability, so it is a tool for repositioning.
public class SwordSkill : PlayerSkill
{
    [Header("Guard")]
    [SerializeField] private Character character;
    [SerializeField] private CharacterStats stats;
    [SerializeField] private float invulnDuration = 4f;
    [SerializeField] private GameObject shieldVisual;   // sits on the player, hidden by default
    [SerializeField] private float speedBonus = 3f;
    [Tooltip("The Swordsman companion. The skill stays unavailable until he has joined.")]
    [SerializeField] private Companion swordsman;

    protected override void Start()
    {
        base.Start();
        if (shieldVisual != null) shieldVisual.SetActive(false);
    }

    // No Swordsman assigned means no Swordsman in this scene, so the skill is unavailable rather
    // than defaulting to on. He is also inactive until Act1Manager hands him over.
    protected override bool IsAvailable =>
        swordsman != null && swordsman.isActiveAndEnabled && swordsman.gameObject.activeInHierarchy;

    protected override IEnumerator Cast()
    {
        character.GrantInvulnerability(invulnDuration);
        if (shieldVisual != null) shieldVisual.SetActive(true);

        stats.moveSpeed += speedBonus;
        try
        {
            yield return new WaitForSeconds(invulnDuration);
        }
        finally
        {
            // finally, because moveSpeed lives on a ScriptableObject and would stay boosted.
            stats.moveSpeed -= speedBonus;
            if (shieldVisual != null) shieldVisual.SetActive(false);
        }
    }
}
