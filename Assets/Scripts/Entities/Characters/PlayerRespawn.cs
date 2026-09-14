using System;
using System.Collections;
using UnityEngine;

// Sends the player back to the start of the scene instead of ending the run.
// This is the only thing that reacts to the player hitting 0 health - Hp.cs just draws the bar.
public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private CharacterStats stats;
    [SerializeField] private SceneState sceneState;
    [Tooltip("Where the player reappears. Leave empty to use wherever they started this scene.")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float respawnDelay = 1f;
    [Tooltip("Fraction of max health restored on respawn. 1 = full.")]
    [Range(0.1f, 1f)][SerializeField] private float healthOnRespawn = 1f;
    [Tooltip("Grace period after reappearing, so a mob standing on the spawn cannot chain-kill.")]
    [SerializeField] private float respawnInvulnerability = 2f;

    // Static so the respawn screen in the HUD prefab can listen without a cross prefab reference
    public static event Action<float> Died;
    public static event Action Respawned;

    private Vector3 startPosition;
    private Rigidbody2D rb;
    private Character character;
    private bool respawning;

    private void Awake()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
    }

    private void Update()
    {
        if (respawning || stats.health > 0) return;
        StartCoroutine(Respawn());
    }

    public void SetSpawnPoint(Transform spawn)
    {
        spawnPoint = spawn;
    }
    private IEnumerator Respawn()
    {
        respawning = true;
        Died?.Invoke(respawnDelay);

        // Character.FixedUpdate already zeroes movement while health is 0, so the player just
        // lies there for the delay rather than sliding around.
        yield return new WaitForSeconds(respawnDelay);

        Vector3 target = spawnPoint != null ? spawnPoint.position : startPosition;
        if (rb != null)
        {
            rb.position = target;
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            transform.position = target;
        }

        stats.health = Mathf.Max(1, Mathf.RoundToInt(stats.maxHealth * healthOnRespawn));
        if (sceneState != null) sceneState.dead = false;
        if (character != null) character.GrantInvulnerability(respawnInvulnerability);

        respawning = false;
        Respawned?.Invoke();
    }
}
