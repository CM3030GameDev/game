using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    private SpriteRenderer sr;
    private Vector2 direction;
    [SerializeField] private SpriteRenderer companionSprite;
    [SerializeField] private GameObject fire;

    // Companion.cs picks the target and owns body facing. This only aims and toggles the flame.
    private Companion companion;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        companion = GetComponentInParent<Companion>();
        // Nothing to burn until an enemy is in range.
        if (fire != null) fire.SetActive(false);
    }

    void Update()
    {
        Transform target = companion != null ? companion.Target : null;

        //Automatically attack if nearby enemy exist
        if (target != null)
        {
            fire.SetActive(true);

            //Vector direction between nearest enemy and mercenary
            direction = target.position - transform.position;

            //Convert angle from radian to degree
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            //Current aim
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //Flip sprite according to aim position
            sr.flipY = target.position.x < transform.position.x;
        }
        //Does not attack if there is no nearby enemy
        else
        {
            fire.SetActive(false);

            // Idle: rest the nozzle along the companion's own facing.
            if (companionSprite.flipX)
            {
                transform.rotation = Quaternion.AngleAxis(0f, Vector3.forward);
                sr.flipY = false;
            }
            else
            {
                transform.rotation = Quaternion.AngleAxis(180f, Vector3.forward);
                sr.flipY = true;
            }
        }
    }
}
