using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedBackground : MonoBehaviour
{
    [SerializeField] private float animatedSpeed = 0.1f;

    private Vector3 startPos;
    private float length;

    private void Awake()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        length = sprite.bounds.size.x;
        startPos = transform.position;
    }

    private void Update()
    {
        transform.Translate(Vector3.right * animatedSpeed * Time.deltaTime);

        if (transform.position.x >= startPos.x + length)
        {
            transform.position = startPos;
        }
    }
}