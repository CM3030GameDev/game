using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private MenuSceneTransition transition;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Opens door for player when they are within range of door after defeating Villain
        if(collision.CompareTag("Character"))
        {
            //Pause everything in the scene except door animation and fade transition
            Time.timeScale = 0f;
            //Play door opening animation
            animator.enabled = true;
            //Fade into darkness to transition to next scene
            transition.LoadSceneWithFade("Act3_Phase2");
        }
    }
}
