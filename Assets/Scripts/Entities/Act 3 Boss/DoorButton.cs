using UnityEngine;

public class DoorButton : MonoBehaviour
{
    //Button interactable state
    private bool interactable;
    [SerializeField] private Door door;
    [SerializeField] private GameObject prompt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(interactable)
        {
            //Player interacts with door using "E" button
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Switch door state when interacted
                door.open = !door.open;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Character"))
        {
            //Show "E" button prompt when player is within range of button
            prompt.SetActive(true);

            //Button can be interacted
            interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Character"))
        {
            //Hide "E" button prompt when player is outside range of button
            prompt.SetActive(false);

            //Button cannot be interacted
            interactable = false;
        }
    }
}
