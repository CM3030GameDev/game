using UnityEngine;

public class MinibossBeamAttack : MonoBehaviour
{
    [Header("Firing Positions")]
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private Transform backPoint;
    [SerializeField] private Transform frontPoint;

    [Header("Beam Settings")]
    public GameObject beamObject;
    [SerializeField] private float beamLifetime = 5f;
    [SerializeField] private int beamDamage = 5;
    [SerializeField] private float beamSpeed = 1f;
    [SerializeField] private int shotsPerAttack = 6;
    [SerializeField] private float playerInvulnerability = 0.1f;

    [Header("Animation")]
    public Animator bossAnimator;
    [Tooltip("attack speed is based on animation speed")]
    public float AttackSpeed = 1f;

    private Transform playerPos;
    private Transform currentPoint;

    private bool beamEnd = false;
    private int shotsFired = 0;
    private string currentDir = "F";

    void Start()
    {
        GameObject player = GameObject.FindGameObjectsWithTag("Character")[0];
        playerPos = player.transform;
        currentPoint = frontPoint;
    }

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
            bossAnimator.SetFloat("AttackSpeed", AttackSpeed);
            bossAnimator.Play(direction + "Attack");
        }
    }

    // Play this animation event every time the boss fires a beam
    public void OnBeamShotEvent()
    {
        if (beamEnd) return;

        SpawnBeam();
        shotsFired++;

        if (shotsFired >= shotsPerAttack)
        {
            beamEnd = true;
            if (bossAnimator != null)
                bossAnimator.Play(currentDir + "Idle"); //stop the loop immediately
        }
    }

    public bool GetBeamEnd()
    {
        return beamEnd;
    }

    private void SpawnBeam()
    {
        if (playerPos == null || beamObject == null)
            return;

        Vector3 spawnPos = currentPoint.position;
        Vector3 direction = playerPos.position - spawnPos;

        GameObject beam = Instantiate(beamObject, spawnPos, Quaternion.identity);
        beam.GetComponent<BeamProjectile>().SetBeam(direction, beamSpeed, beamLifetime, beamDamage, playerInvulnerability);
    }
}