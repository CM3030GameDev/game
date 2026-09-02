using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundSmash : MonoBehaviour
{
    private Animator animator;
    private bool canAttack;
    //Gameobject pool for impact prefab attack
    public Queue<GameObject> impacts;
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject impactPrefab;
    //Speed of impact chasing after player (Must be above 0)
    [SerializeField] private float impactSpeed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        impacts = new Queue<GameObject>();
        //Populate gameobject pool with 10 ground impact prefabs
        for(int i = 0; i < 10; i++)
        {   
            //Instantiate ground impact gameobject
            GameObject impact = Instantiate(impactPrefab, transform.position, Quaternion.identity);
            //Disable newly instantiated ground impact gameobject
            impact.SetActive(false);
            //Add to gameobject pool
            impacts.Enqueue(impact);
        }
    }

    private void OnEnable()
    {
        canAttack = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Ground smash attack ends after 20 animation cycle
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 20f)
        {
            //Disable ground smash attack
            gameObject.SetActive(false);
            //Boss smash attack animation ends
            bossAnimator.SetBool("smash", false);
        }
        //Continue ground smash attack
        else
        {
            if (canAttack)
            {
                StartCoroutine(ImpactTime(impactSpeed));
            }
        }
    }

    IEnumerator ImpactTime(float seconds)
    {
        canAttack = false;
        Vector3 targetPos = character.transform.position;
        yield return new WaitForSeconds(seconds);
        //Spawn ground impact gameobject from object pool
        GameObject impact = impacts.Dequeue();
        //Ground impact appears at player's last position
        impact.transform.position = targetPos;
        impact.SetActive(true);
        impacts.Enqueue(impact);
        canAttack = true;
    }
}
