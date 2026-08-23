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
    public float AttackSpeed = 1f; // tune in Inspector - higher = faster clip playback

    private Transform playerPos;
    private bool isFiring = false;
    private bool hasFired = false;

    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private Transform backPoint;
    [SerializeField] private Transform frontPoint;

    private Transform currentPoint;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectsWithTag("Character")[0];
        playerPos = player.transform;
        currentPoint = frontPoint;
    }


    private int r = 0;


    private int shotsFired = 0;
    private int shotsPerAttack = 3;
    private bool beamEnd = false;
    private string currentDir = "F";

    public void EnableBeamAttack(string direction)
    {
        currentDir = direction;
        shotsFired = 0;
        beamEnd = false;

        switch (direction)
        {
            case "F": currentPoint = frontPoint; break;
            case "B": currentPoint = backPoint; break;
            case "L": currentPoint = leftPoint; break;
            case "R": currentPoint = rightPoint; break;
            default: currentPoint = frontPoint; break;
        }

        if (bossAnimator != null)
        {
            bossAnimator.SetFloat(attackSpeedParam, 1f);
            bossAnimator.Play(direction + "Attack");
        }
    }

    // Animation Event, placed once, repeats each time the looping clip cycles
    public void OnBeamShotEvent()
    {
        if (beamEnd) return; // already done, ignore any late/extra calls

        SpawnBeam();
        shotsFired++;

        if (shotsFired >= shotsPerAttack)
        {
            beamEnd = true;
            if (bossAnimator != null)
                bossAnimator.Play(currentDir + "Idle"); // stop the loop immediately
        }
    }

    public bool GetBeamEnd() => beamEnd;

    // Hook this up as an Animation Event at the very end of the beam attack clip

    public void OnBeamAttackEndEvent()
    {
        isFiring = false;
        hasFired = true;
    }

    private void SpawnBeam()
    {
        if (playerPos == null || beamObject == null) return;

        Vector3 spawnPos = currentPoint.position;
        Vector3 direction = playerPos.position - spawnPos; // re-aims fresh each shot

        GameObject beam = Instantiate(beamObject, spawnPos, Quaternion.identity);
        beam.GetComponent<BeamProjectile>().SetDirection(direction);
    }

    public bool GetIsFiring() => isFiring;
    public void SetIsFiring(bool setFiring) => isFiring = setFiring;
    public bool GetHasFired() => hasFired;
}