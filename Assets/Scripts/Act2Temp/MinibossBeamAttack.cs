using UnityEngine;

public class MinibossBeamAttack : MonoBehaviour
{
    [Header("Beam Settings")]
    public GameObject beamObject;
    public Transform shootPoint;

    [Header("Animation")]
    public Animator bossAnimator;
    public string beamAttackAnimName = "BeamAttack";
    public string attackSpeedParam = "AttackSpeed"; // Animator float parameter name
    public float attackSpeed = 1f; // tune in Inspector - higher = faster clip playback

    private Transform playerPos;
    private bool isFiring = false;
    private bool hasFired = false;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectsWithTag("Character")[0];
        playerPos = player.transform;
    }

    public void EnableBeamAttack()
    {
        if (isFiring) return;

        isFiring = true;
        hasFired = false;

        if (bossAnimator != null)
        {
            bossAnimator.SetFloat(attackSpeedParam, attackSpeed);
            bossAnimator.Play(beamAttackAnimName);
        }
        // no coroutine/loop here anymore - the animation itself calls
        // OnBeamShotEvent() and OnBeamAttackEndEvent() via Animation Events
    }

    // Hook this up as an Animation Event, once per shot, at the exact frame each shot should fire
    public void OnBeamShotEvent()
    {
        SpawnBeam();
    }

    // Hook this up as an Animation Event at the very end of the beam attack clip
    public void OnBeamAttackEndEvent()
    {
        isFiring = false;
        hasFired = true;
    }

    private void SpawnBeam()
    {
        if (playerPos == null || beamObject == null) return;

        Vector3 spawnPos = shootPoint != null ? shootPoint.position : transform.position;
        Vector3 direction = playerPos.position - spawnPos; // re-aims fresh each shot

        GameObject beam = Instantiate(beamObject, spawnPos, Quaternion.identity);
        beam.GetComponent<BeamProjectile>().SetDirection(direction);
    }

    public bool GetIsFiring() => isFiring;
    public void SetIsFiring(bool setFiring) => isFiring = setFiring;
    public bool GetHasFired() => hasFired;
}