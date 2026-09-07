using UnityEngine;

public class MinibossAttackRange : MonoBehaviour
{
    private bool isInRange = false;
    private GameObject target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            isInRange = true;
            //target not set to null onExit so that it is a guaranteed hit
            target = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            isInRange = false;
        }
    }

    public bool GetIsInRange()
    {
        return isInRange;
    }

    public GameObject GetTarget()
    {
        return target;
    }
}
