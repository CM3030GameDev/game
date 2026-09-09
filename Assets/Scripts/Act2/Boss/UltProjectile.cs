using UnityEngine;

public class UltProjectile : MonoBehaviour
{
    private Animator anim;
    private int damage = 15;
    private float lifetime = 5f;
    private float playerInvulnerability = 0.1f;
    private float speed;

    StateTrigger exitTrigger;
    private Vector3 moveDir;
    private bool isLaunched = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        if (anim != null)
            exitTrigger = anim.GetBehaviour<StateTrigger>();
    }

    public void Launch(Vector3 direction, float launchSpeed, float lifeTimer, int damageDone, float invulnerability)
    {
        damage = damageDone;
        lifetime = lifeTimer;
        moveDir = direction;
        speed = launchSpeed;
        playerInvulnerability = invulnerability;
        isLaunched = true;
        if(anim != null)
        {
            anim.Play("UltCharging");

            if (exitTrigger != null)
            {
                exitTrigger.OnStateExitAction = null;
                exitTrigger.OnStateExitAction += OnAnimationEnd;
            }
        }
    }

    void Update()
    {
        if (isLaunched)
        {
            transform.position += moveDir * speed * Time.deltaTime;
            if(lifetime > 0f)
            {
                lifetime -= Time.deltaTime;
            }
            else if(lifetime <= 0f)
            {
                isLaunched = false;
                UltExplode();
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
                character.CharacterAttacked(damage);
                character.GrantInvulnerability(playerInvulnerability);
            }
            UIAudioManager.Instance.PlaySFXOneShot(7);
            UltExplode();
        }
    }

    private void UltExplode()
    {
        isLaunched = false;
        anim.Play("UltExplode");
        UIAudioManager.Instance.PlaySFXOneShot(5);
    }

    private void OnAnimationEnd()
    {
        exitTrigger.OnStateExitAction = null;
        Destroy(gameObject);
    }
}
