using UnityEngine;

public class Beams : MonoBehaviour
{
    //Positive 1 represent anti-clockwise, negative 1 represent clockwise
    public float direction;

    private void OnEnable()
    {
        //Randomize starting rotation to a value between 0 and 360 degree
        transform.rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);

        //Randomize direction (Anti-clockwise or clockwise direction)
        if(Random.Range(0f, 1f) > 0.5f)
        {
            direction = 1f;
        }
        else
        {
            direction = -1f;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
