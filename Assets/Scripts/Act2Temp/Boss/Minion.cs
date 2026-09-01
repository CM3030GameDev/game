using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Minion : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer warningBox;
    [SerializeField] private float growthSpeed = 20f;
    [SerializeField] private float maxLength = 80f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private int damage = 5;

    private Vector3 startPos;
    private bool hasStartPos = false;

    private DirectionFacing dir = DirectionFacing.LEFT;
    private MinionState currentState = MinionState.WARNING;

    private Vector3 moveDir;
    private bool isInitialized = false;
    private float worldSpaceTravelDistance;

    public enum DirectionFacing
    {
        LEFT,
        TOP
    }

    private enum MinionState
    {
        WARNING,
        MOVING,
        END
    }

    void Update()
    {
        if (!isInitialized) return;

        if(!hasStartPos)
        {
            startPos = transform.position;
            hasStartPos = true;
        }

        switch (currentState)
        {
            case MinionState.WARNING:
                HandleWarning();
                break;

            case MinionState.MOVING:
                HandleMoving();
                break;
            case MinionState.END:
                HandleEnd();
                break;
            default:
                break;
        }
    }

    private void HandleWarning()
    {
        if (dir == DirectionFacing.LEFT)
        {
            if (warningBox.size.x < maxLength)
            {
                float oldWidth = warningBox.size.x;
                float newWidth = Mathf.Min(oldWidth + (growthSpeed * Time.deltaTime), maxLength);
                warningBox.size = new Vector2(newWidth, warningBox.size.y);

                float widthDifference = newWidth - oldWidth;
                float localOffset = (widthDifference * warningBox.transform.localScale.x) / 2f;
                warningBox.transform.localPosition += new Vector3(localOffset, 0, 0);
            }
            else
            {
                worldSpaceTravelDistance = warningBox.size.x * warningBox.transform.lossyScale.x;
                currentState = MinionState.MOVING;
                warningBox.transform.SetParent(null);
            }
        }
        else if (dir == DirectionFacing.TOP)
        {
            if (warningBox.size.y < maxLength)
            {
                float oldHeight = warningBox.size.y;
                float newHeight = Mathf.Min(oldHeight + (growthSpeed * Time.deltaTime), maxLength);
                warningBox.size = new Vector2(warningBox.size.x, newHeight);

                float heightDifference = newHeight - oldHeight;
                float localOffset = (heightDifference * warningBox.transform.localScale.y) / 2f;
                warningBox.transform.localPosition += new Vector3(0, -localOffset, 0);
            }
            else
            {
                worldSpaceTravelDistance = warningBox.size.y * warningBox.transform.lossyScale.y;
                currentState = MinionState.MOVING;
                warningBox.transform.SetParent(null);
            }
        }
    }

    private void HandleMoving()
    {
        if (dir == DirectionFacing.LEFT)
            moveDir = Vector3.right;
        else if (dir == DirectionFacing.TOP)
            moveDir = Vector3.down;

        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float distanceTraveled = Vector3.Distance(startPos, transform.position);

        if (distanceTraveled >= worldSpaceTravelDistance) // compare against actual world length, not raw maxLength
        {
            currentState = MinionState.END;
        }
    }

    private void HandleEnd()
    {
        Destroy(warningBox.gameObject);
        Destroy(gameObject);
    }

    public void Initialize(DirectionFacing df, float speed, float growth, int dmg, float maxlength)
    {
        maxLength = maxlength;
        moveSpeed = speed;
        growthSpeed = growth;
        damage = dmg;
        dir = df;
        isInitialized = true;
        ApplyDirection();
    }

    private void ApplyDirection()
    {
        if (anim == null) return;

        if (dir == DirectionFacing.LEFT)
        {
            anim.Play("RCRun");
        }
        else if (dir == DirectionFacing.TOP)
        {
            anim.Play("FCRun");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character") && collision.isTrigger)
        {
            Character character = collision.gameObject.GetComponent<Character>();
            if (character != null)
                character.CharacterAttacked(damage);
        }
    }
}
