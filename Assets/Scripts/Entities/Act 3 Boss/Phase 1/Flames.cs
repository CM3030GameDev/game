using UnityEngine;

public class Flames : MonoBehaviour
{
    private float currentTime;
    //Check if fire hydrant hint has been given already
    private bool hydrantHint;
    //Flame current health
    private int currentHP;
    [SerializeField] private int flameHP;
    [SerializeField] private int damage;
    [SerializeField] private DialogueData hint;
    [SerializeField] private DialogueData counterAttack;
    [SerializeField] private string extinguishTask;
    [SerializeField] private string extinguishedTask;

    private void OnEnable()
    {
        currentTime = 0f;
        hydrantHint = false;
        currentHP = flameHP;

        //Mission UI task appear to prompt player to extinguish boss flames
        PhaseOneManager.Instance.missionUI.SetTasks(extinguishTask);
        //Give hint via dialogue prompt that water source is needed to extinguish boss flames
        DialogueManager.Instance.StartDialogue(hint);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Disable flames if health is below 0
        if(flameHP <= 0)
        {
            //Mission prompt to show that player has cleared task
            PhaseOneManager.Instance.missionUI.SetTasks($"<s>{extinguishedTask}</s>");
            //Inform player via dialogue prompt that flame has been extinguished and villain can be attacked again
            DialogueManager.Instance.StartDialogue(counterAttack);
            //Flame is extinguished
            gameObject.SetActive(false);
        }

        currentTime += Time.deltaTime;

        //Inform player through dialogue prompt that the fire hydrant is a water source if they have yet to break a hydrant after a period of time
        if (!hydrantHint && !PhaseOneManager.Instance.hydrantHint && currentTime > 20f)
        {
            hydrantHint = true;
            DialogueManager.Instance.StartDialogue(hint);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bullet"))
        {
            //Destroy bullet when it collides with the flames
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Decrease flame health when in contact with fire hydrant's water
        if(collision.CompareTag("Water"))
        {
            currentHP -= 10;
        }
        else if (collision.CompareTag("Character"))
        {
            Character character = collision.GetComponent<Character>();
            character.CharacterAttacked(damage, 0.05f);
        }
    }
}
