using UnityEngine;

public class Flames : MonoBehaviour
{
    private float currentTime;
    //Check if fire hydrant hint has been given already
    private bool hydrantHint;
    //Check if at least one first hydrant has been broken
    public bool broken;
    //Flame current health
    private int currentHP;
    [SerializeField] private int flameHP;
    [SerializeField] private int damage;
    [SerializeField] private DialogueData hint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = 0f;
        hydrantHint = false;
        broken = false;
        currentHP = flameHP;

        //Give hint via dialogue prompt that water source is needed to extinguish boss flames
        DialogueManager.Instance.StartDialogue(hint);
    }

    // Update is called once per frame
    void Update()
    {
        //Disable flames if health is below 0
        if(flameHP <= 0)
        {
            //Flame is extinguished
            gameObject.SetActive(false);
        }

        currentTime += Time.deltaTime;

        //Inform player through dialogue prompt that the fire hydrant is a water source (To help players if they are stucked)
        if (!hydrantHint && !broken && currentTime > 30f)
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
            currentHP -= 1;
        }
        else if (collision.CompareTag("Character"))
        {
            Character character = collision.GetComponent<Character>();
            character.CharacterAttacked(damage);
            character.GrantInvulnerability(0.05f);
        }
    }
}
