using UnityEngine;

public class Barrier : MonoBehaviour
{
    private float currentTime;
    //Barrier timer
    [SerializeField] private float barrierTime;

    private void OnEnable()
    {
        currentTime = 0f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime > barrierTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            //Incoming attack's rigidbody
            Rigidbody2D rb = collision.attachedRigidbody;
            //Direct incoming attack in the opposite direction at the same speed
            rb.linearVelocity = rb.linearVelocity * -1;
        }
    }
}
