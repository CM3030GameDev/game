using UnityEngine;

public class PhaseTwoDash : MonoBehaviour
{
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private FinalBossTwo finalBossTwo;
    [SerializeField] private Animator animator;

    private void OnEnable()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Stop dashing when close to player current position
        if (Vector2.Distance(transform.position, PhaseTwoManager.Instance.characterTransform.position) < 5f)
        {
            finalBossTwo.dashing = false;
            gameObject.SetActive(false);
            //Revert boss speed to original speed
            finalBossTwo.currentSpeed = characterStats.moveSpeed - 2;
            //End boss dashing animation
            animator.SetBool("dash", false);
        }
    }
}
