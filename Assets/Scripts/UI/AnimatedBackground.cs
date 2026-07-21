using UnityEngine;

public class AnimatedBackground : MonoBehaviour
{
    private Vector3 startPos;
    private float length;
    [SerializeField] private float animatedSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        //Animation speed
        transform.position += new Vector3(animatedSpeed * Time.deltaTime, 0, 0);


        //Reset back to original position if it goes beyond the border
        if (transform.position.x > startPos.x + length)
        {
            transform.position = startPos;
        }
    }
}
