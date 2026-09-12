using UnityEngine;
using System.Collections;

public class Barrier : MonoBehaviour
{
    [SerializeField] private FinalBossTwo finalBossTwo;
    //Barrier timer
    [SerializeField] private float duration;

    private void OnEnable()
    {
        StartCoroutine(BarrierTime(duration));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            //Incoming attack's rigidbody
            Rigidbody2D rb = collision.attachedRigidbody;
            //Direct incoming attack in the opposite direction at the same speed
            rb.linearVelocity = rb.linearVelocity * -1;

            //Update reflect state of bullet to be true
            collision.GetComponent<Bullet>().MarkReflected();
        }
    }

    IEnumerator BarrierTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
}
