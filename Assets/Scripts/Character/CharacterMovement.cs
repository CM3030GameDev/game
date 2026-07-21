using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D characterRB;
    [SerializeField] private Animator characterSprite;
    [SerializeField] private RuntimeAnimatorController soldierSprite;
    [SerializeField] private RuntimeAnimatorController mercenarySprite;
    [SerializeField] private RuntimeAnimatorController swordsmanSprite;
    [SerializeField] private CompanionSystem companionSystem;
    private Camera mainCamera;
    private float inputX;
    private float inputY;
    private Vector2 movement;
    //Invulnerable duration
    private float attackedDuration;
    //Invulnerable state
    private bool isAttacked;
    //private float increment;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        isAttacked = false;
        //increment = 0;
    }

    // Update is called once per frame
    void Update()
    {
        companionSystem.ManualSwap();
        companionSystem.AutoSwap();

        //Update animation sprite if "Q" or "E" button pressed
        if (companionSystem.companionSwap)
        {
            if (companionSystem.playerCharacter == "soldier")
            {
                characterSprite.runtimeAnimatorController = soldierSprite;
                companionSystem.companionSwap = false;
            }
            else if (companionSystem.playerCharacter == "mercenary")
            {
                characterSprite.runtimeAnimatorController = mercenarySprite;
                companionSystem.companionSwap = false;
            }
            else if (companionSystem.playerCharacter == "swordsman")
            {
                characterSprite.runtimeAnimatorController = swordsmanSprite;
                companionSystem.companionSwap = false;
            }
        }
        Movement();
        WeaponAim();

        attackedDuration -= Time.deltaTime;

        if (attackedDuration < 0)
        {
            isAttacked = false;
            characterSprite.SetBool("attacked", false);
        }

        //Spin swordsman's sprite (For special skill purpose)
        //increment += 0.05f;
        //if (increment > 4)
        //{
        //    increment = 0;
        //}
        //characterSprite.SetFloat("direction", increment);
    }

    private void FixedUpdate()
    {
        characterRB.linearVelocity = movement * companionSystem.characterMS;
    }

    private void Movement()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        //Normalize diagonal direction vector to ensure it has magnitude of 1
        movement = new Vector2(inputX, inputY).normalized;

        if (inputX != 0 || inputY != 0)
        {
            characterSprite.SetBool("move", true);
        }
        else
        {
            characterSprite.SetBool("move", false);
        }
    }

    private void WeaponAim()
    {
        //Get mouse position in screen pixels
        Vector3 screenPosition = Input.mousePosition;

        //Convert screen pixels to 3D world coordinates
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

        //Convert 3D world coordinate to 2D 
        worldPosition.z = 0f;

        //Direction vector from player to mouse
        Vector2 towards = worldPosition - transform.position;

        //Convert angle from radian to degree
        float angle = Mathf.Atan2(towards.y, towards.x) * Mathf.Rad2Deg;

        //Offset angle
        angle += 45f;

        //Convert negative angle to positve angle
        if (angle < 0)
        {
            angle += 360;
        }

        //Determine which direction player face
        int direction = Mathf.FloorToInt(angle / 90);

        //Face right
        if (direction == 0)
        {
            characterSprite.SetFloat("direction", 0);
        }
        //Face upward
        else if (direction == 1)
        {
            characterSprite.SetFloat("direction", 1);
        }
        //Face left
        else if (direction == 2)
        {
            characterSprite.SetFloat("direction", 2);
        }
        //Face downward
        else if (direction == 3)
        {
            characterSprite.SetFloat("direction", 3);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isAttacked)
        {
            isAttacked = true;
            //Attacked animation of character
            characterSprite.SetBool("attacked", true);
            attackedDuration = 0.1f;

            //If player is currently soldier
            if (companionSystem.playerCharacter == "soldier")
            {
                companionSystem.soldierHealth -= 10;
            }
            //If player is currently mercenary
            else if (companionSystem.playerCharacter == "mercenary")
            {
                companionSystem.mercenaryHealth -= 10;
            }
            //If player is currently swordsman
            else if (companionSystem.playerCharacter == "swordsman")
            {
                companionSystem.swordsmanHealth -= 10;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isAttacked)
        {
            isAttacked = true;
            //Attacked animation of character
            characterSprite.SetBool("attacked", true);
            attackedDuration = 0.1f;

            //If player is currently soldier
            if (companionSystem.playerCharacter == "soldier")
            {
                companionSystem.soldierHealth -= 10;
            }
            //If player is currently mercenary
            else if (companionSystem.playerCharacter == "mercenary")
            {
                companionSystem.mercenaryHealth -= 10;
            }
            //If player is currently swordsman
            else if (companionSystem.playerCharacter == "swordsman")
            {
                companionSystem.swordsmanHealth -= 10;
            }
        }
    }
}