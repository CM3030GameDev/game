using UnityEngine;

public class Beam : MonoBehaviour
{
    [SerializeField] private FinalBoss finalBoss;
    private float currentRotation;

    private void OnEnable()
    {
        //Beam starts from left
        if(finalBoss.beamDirection)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        //Beam starts from right
        else
        {
            transform.rotation = Quaternion.identity;
        }

        //Reset rotation to 0
        currentRotation = 0f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If beam has yet to rotate a full circle, keep rotating
        if(currentRotation < 360f)
        {
            //Beam starts left and ends left
            if (finalBoss.beamDirection)
            {
                //Rotates beam by 360 degrees
                transform.Rotate(Vector3.forward * 100f * Time.deltaTime);
                //Increment rotation value
                currentRotation += 100f * Time.deltaTime;
            }
            //Beam starts right and ends right
            else
            {
                //Rotates beam by 360 degrees
                transform.Rotate(Vector3.forward * 100f * Time.deltaTime);
                //Increment rotation value
                currentRotation += 100f * Time.deltaTime;
            }
        }
        //Stop beam after rotating a full circle
        else
        {
            gameObject.SetActive(false);
            finalBoss.GetComponent<Animator>().SetTrigger("beam_end");
            finalBoss.attacking = false;
        }
    }
}
