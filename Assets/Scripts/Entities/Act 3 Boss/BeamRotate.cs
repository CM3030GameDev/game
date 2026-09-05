using UnityEngine;

public class BeamRotate : MonoBehaviour
{
    //Rotation count
    private float currentRotation;
    //True represent start rotating, false represent stop rotating
    public bool isRotating;
    [SerializeField] private Beams beams;
    [SerializeField] private GameObject beamBody;
    [SerializeField] private Animator beamAnimator;
    //Beam charge box collider
    [SerializeField] private BoxCollider2D chargeCollider;
    //Beam body box collider
    [SerializeField] private BoxCollider2D bodyCollider;
    [SerializeField] private float beamSpeed;

    private void OnEnable()
    {
        //Reset rotation count to 0
        currentRotation = 0f;
        //Enable beam body to show hitbox indicator
        beamBody.SetActive(true);
        //Do not start rotating until beam charge animation finishes
        isRotating = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotating)
        {
            //Keep rotating beam until a full circle is completed
            if (currentRotation < 360f)
            {
                //Rotates beam by 360 degrees
                transform.Rotate(Vector3.forward * beamSpeed * beams.direction * Time.deltaTime);
                //Increment rotation value
                currentRotation += beamSpeed * Time.deltaTime;
            }
            //Stop beam after rotating a full circle
            else
            {
                //Disable beam charge box collider
                chargeCollider.enabled = false;
                //Disable beam body
                beamBody.SetActive(false);
                //Beam ending animation
                beamAnimator.SetTrigger("end");
            }
        }
    }
}
