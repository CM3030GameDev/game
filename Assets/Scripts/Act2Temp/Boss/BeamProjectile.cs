using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BeamProjectile : MonoBehaviour
{
    private Vector3 moveDir;
    private float speed = 25f;
    private float lifetime = 5f;
    private int damage = 5;
    private bool isInitialized = false;

    public void SetBeam(Vector3 direction, float moveSpeed, float timer, int damageDone)
    {
        lifetime = timer;
        damage = damageDone;
        moveDir = direction.normalized;
        speed = moveSpeed;
        isInitialized = true;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (!isInitialized) return;

            transform.position += moveDir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            Character character = collision.gameObject.GetComponent<Character>();
            if (character != null)
                character.CharacterAttacked(damage);

            Destroy(gameObject);
        }
    }
}
