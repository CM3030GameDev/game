using UnityEngine;

public class Swordsman : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    [SerializeField] private GameObject character;
    [SerializeField] private float followRange = 10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(character.transform.position, transform.position);
        if (distance > followRange)
        {
            //test
        }
    }
}
