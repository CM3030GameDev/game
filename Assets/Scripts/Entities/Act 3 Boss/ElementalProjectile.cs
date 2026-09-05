using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalProjectile : MonoBehaviour
{
    private bool canAttack;
    private int counts;
    //List for elemental projectile prefabs
    [SerializeField] private List<GameObject> projectiles = new List<GameObject>();
    [SerializeField] private Transform characterPos;
    [SerializeField] private Animator animator;
    [SerializeField] private float projectileSpeed;
    //Time between each projectile attack
    [SerializeField] private float interval;

    private void OnEnable()
    {
        //3 consecutive projectile attacks
        counts = 3;
        canAttack = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Continue shooting projectile
        if (counts > 0)
        {
            if (canAttack)
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
        canAttack = false;
        //Decrease projectile attack count left by 1
        counts--;
        //Direction vector from boss to player
        Vector2 direction = characterPos.position - transform.position;
        //Normalized direction vector from boss to player
        Vector2 directionNormalized = direction.normalized;
        //Choose a random element projectile to shoot at player
        GameObject projectile = Instantiate(projectiles[Random.Range(0, projectiles.Count)], transform.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        //Shoot projectile in the direction of player
        rb.linearVelocity = direction * projectileSpeed;
        yield return new WaitForSeconds(seconds);
        canAttack = true;
    }
}
