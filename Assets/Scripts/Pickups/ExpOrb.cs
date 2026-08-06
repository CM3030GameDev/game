using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private int expValue = 10;        // Flat Value (change later!!)
    [SerializeField] private float pickupRadius = 2f;
    [SerializeField] private float moveSpeed = 8f;      // Move speed of the exp orb when the player is in range
    [SerializeField] private float collectDistance = 0.3f;

    private Transform player;

    private void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Character");
        if (p != null) player = p.transform;
    }

    private void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        // Magnet effect when ExpOrb is within the player's pickup radius
        if (dist <= pickupRadius)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, player.position, moveSpeed * Time.deltaTime);
        }

        // Collects when close enough and deletes the exp orb once collected
        if (dist <= collectDistance)
        {
            characterStats.expPoint += expValue;
            Destroy(gameObject);
        }
    }

    // Exp Value of each exp orb
    public void SetExp(int value)
    {
        expValue = value;
    }
}