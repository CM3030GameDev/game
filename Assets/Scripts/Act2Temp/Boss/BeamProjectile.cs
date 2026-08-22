using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BeamProjectile : MonoBehaviour
{
    public float speed = 12f;
    public float lifetime = 5f; // safety despawn if it never hits anything or leaves the screen
    public int damage = 5;

    private Vector3 moveDir;
    private bool isInitialized = false;

    // Call this right after Instantiate to set travel direction
    public void SetDirection(Vector3 direction)
    {
        moveDir = direction.normalized;
        isInitialized = true;
        Destroy(gameObject, lifetime); // auto-cleanup safety net
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
