using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BeamProjectile : MonoBehaviour
{
    private Vector3 moveDir;
    private float speed = 25f;
    private float lifetime = 5f;
    private int damage = 5;
    private float playerInvulnerability = 0.1f;
    private Animator anim;
    private bool isInitialized = false;
    StateTrigger exitTrigger;
    private bool isLaunched = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        if (anim != null)
            exitTrigger = anim.GetBehaviour<StateTrigger>();
    }
    public void SetBeam(Vector3 direction, float moveSpeed, float timer, int damageDone, float invulnerability)
    {
        lifetime = timer;
        damage = damageDone;
        moveDir = direction.normalized;
        speed = moveSpeed;
        playerInvulnerability = invulnerability;
        isInitialized = true;
        isLaunched = true;

        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (anim != null)
        {
            anim.Play("SkillShot");

            if (exitTrigger != null)
            {
                exitTrigger.OnStateExitAction = null;
                exitTrigger.OnStateExitAction += OnAnimationEnd;
            }
        }
    }

    void Update()
    {
        if (!isInitialized)
            return;

        if (isLaunched)
        {
            transform.position += moveDir * speed * Time.deltaTime;
            if (lifetime > 0f)
            {
                lifetime -= Time.deltaTime;
            }
            else if (lifetime <= 0f)
            {
                isLaunched = false;
                isInitialized = false;
                BeamExplode();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            Character character = collision.gameObject.GetComponent<Character>();
            if (!character.isAttacked && character != null)
            {
                character.CharacterAttacked(damage, playerInvulnerability);
                UIAudioManager.Instance.PlaySFXOneShot(6);
            }
            BeamExplode();
        }
    }

    private void BeamExplode()
    {
        isLaunched = false;
        anim.Play("SkillImpact");
    }
    private void OnAnimationEnd()
    {
        exitTrigger.OnStateExitAction = null;
        Destroy(gameObject);
    }
}
