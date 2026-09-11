using UnityEngine;
using System.Collections;

public class StunnedEffect : MonoBehaviour
{
    private float originalSpeed;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        originalSpeed = characterStats.moveSpeed;
        characterStats.moveSpeed = 0f;
        StartCoroutine(StatusDuration(3f));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        //Set player to be in idle animation while stunned
        animator.SetBool("move", false);
    }

    IEnumerator StatusDuration(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        //Set speed back to original speed
        characterStats.moveSpeed = originalSpeed;
        gameObject.SetActive(false);
    }
}
