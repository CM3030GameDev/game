using UnityEngine;

public class Fog : MonoBehaviour
{
    private Vector3 startPos;
    [SerializeField] private float fogSpeed;

    private void Awake()
    {
        startPos = transform.localPosition;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localPosition.x >= 41f)
        {
            //Reset fog back to original position
            transform.localPosition = startPos;
        }
        else
        {
            //Move fog to the right
            transform.Translate(Vector3.right * fogSpeed * Time.deltaTime);
        }
    }
}
