using UnityEngine;

public class BeamRotate : MonoBehaviour
{
    //Rotation count
    private float currentRotation;
    //Positive 1 represent anti-clockwise, negative 1 represent clockwise
    private float direction;
    //True represent start rotating, false represent stop rotating
    public bool isRotating;
    [SerializeField] private FinalBoss finalBoss;
    [SerializeField] private Animator beamAnimator1;
    [SerializeField] private Animator beamAnimator2;
    [SerializeField] private Animator beamAnimator3;
    [SerializeField] private Animator beamAnimator4;
    [SerializeField] private GameObject beam1;
    [SerializeField] private GameObject beam2;
    [SerializeField] private GameObject beam3;
    [SerializeField] private GameObject beam4;
    [SerializeField] private float beamSpeed;

    private void OnEnable()
    {
        //Reset rotation count to 0
        currentRotation = 0f;

        isRotating = false;

        //Set rotation to a random value between 0 and 360 degree
        transform.rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);

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
        if(isRotating)
        {
            //If beam has yet to rotate a full circle, keep rotating
            if (currentRotation < 360f)
            {
                //Rotates beam by 360 degrees
                transform.Rotate(Vector3.forward * beamSpeed * direction * Time.deltaTime);
                //Increment rotation value
                currentRotation += beamSpeed * Time.deltaTime;
            }
            //Stop beam after rotating a full circle
            else
            {
                finalBoss.randomNum = Random.Range(0f, 1f);
                finalBoss.attackTime = 0f;
                finalBoss.GetComponent<Animator>().SetTrigger("beam_end");
                beamAnimator1.SetTrigger("end");
                beamAnimator2.SetTrigger("end");
                beamAnimator3.SetTrigger("end");
                beamAnimator4.SetTrigger("end");
                beam1.transform.localScale = new Vector3(0f, 2f, 1f);
                beam2.transform.localScale = new Vector3(0f, 2f, 1f);
                beam3.transform.localScale = new Vector3(0f, 2f, 1f);
                beam4.transform.localScale = new Vector3(0f, 2f, 1f);
                finalBoss.attacking = false;
                gameObject.SetActive(false);
            }
        }
    }
}
