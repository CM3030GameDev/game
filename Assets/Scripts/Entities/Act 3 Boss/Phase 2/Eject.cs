using UnityEngine;

public class Eject : MonoBehaviour
{
    private bool push;
    [SerializeField] private Door door;
    [SerializeField] private Rigidbody2D characterRB;
    [SerializeField] private float force;

    private void Awake()
    {
        push = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(push)
        {
            characterRB.AddForce(transform.right * force, ForceMode2D.Force);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Forces player out of the safe room if they are still inside after beam attack has ended
        if (collision.CompareTag("Character"))
        {
            push = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Close door automatically when player gets ejected
        if(collision.CompareTag("Character"))
        {
            //Stop pushing
            push = false;
            //Closes door
            door.open = false;
        }
    }
}
