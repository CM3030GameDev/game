using UnityEngine;

public class Hydrant : MonoBehaviour
{
    private Animator animator;
    private bool leak;
    public int hydrantHP;
    [SerializeField] private GameObject puddle;
    [SerializeField] private DialogueData hint;
    [SerializeField] private string puddleTask;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        leak = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!leak)
        {
            if(hydrantHP <= 0)
            {
                leak = true;
                //Start hydrant leakage animation
                animator.SetTrigger("leak");

                //Inform player via dialogue prompt that we have to lure the boss flames into the fire hydrant puddle to slowly extinguish it
                if(!PhaseOneManager.Instance.hydrantHint)
                {
                    PhaseOneManager.Instance.hydrantHint = true;
                    //Mission task prompt to inform player how to extinguish boss flame using fire hydrant puddle
                    PhaseOneManager.Instance.missionUI.SetTasks(puddleTask);
                    //Dialogue only appears after the first hydrant has been broken (Subsequent hydrant will not)
                    DialogueManager.Instance.StartDialogue(hint);
                }
            }
        }
        else
        {
            if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                puddle.SetActive(true);
            }
        }
    }
}
