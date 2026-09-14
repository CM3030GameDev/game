using System.Collections;
using UnityEngine;

public class ElementalProjectile : MonoBehaviour
{
    private int projectileCount;
    private bool shooting;
    [SerializeField] private Animator animator;
    [SerializeField] private float projectileSpeed;
    //Time between each projectile attack
    [SerializeField] private float interval;
    private void OnEnable()
    {
        //3 consecutive different element projectile attacks
        projectileCount = 3;
        shooting = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Continue shooting projectile
        if (projectileCount > 0)
        {
            if (!shooting)
            {
                StartCoroutine(ShootTime(interval));
            }
        }
        //Stop shooting projectile
        else
        {
            //Disable elemental projectile attack
            gameObject.SetActive(false);
            //Boss attack animation ends
            animator.SetBool("attack", false);
        }
    }

    IEnumerator ShootTime(float seconds)
    {
        shooting = true;
        //Decrease projectile attack count left by 1
        projectileCount--;
        //Direction vector from boss to player
        Vector2 direction = PhaseTwoManager.Instance.characterTransform.position - transform.position;
        //Normalized direction vector from boss to player
        Vector2 directionNormalized = direction.normalized;
        //Get element projectile gameobject from object pool
        GameObject projectile = PhaseTwoManager.Instance.projectiles.Dequeue();
        //Shoot projectile from boss position
        projectile.transform.position = transform.position;
        //Projectile appears
        projectile.SetActive(true);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        //Shoot projectile in the direction of player
        rb.linearVelocity = directionNormalized * projectileSpeed;
        //Play projectile sound effect
        UIAudioManager.Instance.PlaySFXOneShot(3);
        yield return new WaitForSeconds(seconds);
        shooting = false;
    }
}
