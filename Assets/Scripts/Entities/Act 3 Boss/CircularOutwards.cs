using UnityEngine;

public class CircularOutwards : MonoBehaviour
{
    [SerializeField] private float circleSpeed;

    private void OnEnable()
    {
        transform.localScale = new Vector3(0f, 0f, 1f);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x >= 1f && transform.localScale.y >= 1f)
        {
            transform.localScale = new Vector3(0f, 0f, 1f);
        }

        transform.localScale += new Vector3(circleSpeed * Time.deltaTime, circleSpeed * Time.deltaTime, 1f);
    }
}
