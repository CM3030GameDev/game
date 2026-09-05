using System.Collections;
using UnityEngine;

// Swordsman - Guard. Defensive: a burst of speed plus a window of invulnerability, so it's a
// reposition tool rather than just a damage sponge. The hyperspeed screen effect is wired
// through PlayerSkill's Screen Effect field.
public class SwordSkill : PlayerSkill
{
    [Header("Guard")]
    [SerializeField] private Character character;
    [SerializeField] private CharacterStats stats;
    [SerializeField] private float invulnDuration = 4f;
    [SerializeField] private GameObject shieldVisual;   // sits on the player, hidden by default
    [SerializeField] private float speedBonus = 3f;

    protected override void Start()
    {
        base.Start();
        if (shieldVisual != null) shieldVisual.SetActive(false);
    }

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
            // finally, because moveSpeed lives on a ScriptableObject - an interrupted cast
            // would otherwise leave the bonus applied permanently.
            stats.moveSpeed -= speedBonus;
            if (shieldVisual != null) shieldVisual.SetActive(false);
        }
    }
}
