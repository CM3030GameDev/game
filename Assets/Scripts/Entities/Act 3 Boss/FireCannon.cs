using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireCannon : MonoBehaviour
{
    //Gameobject pool for fire prefab attack
    private Queue<GameObject> fires;
    //Number of fire attack left
    private int counts;
    private bool canAttack;
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private GameObject character;
    [SerializeField] private Animator animator;
    //Fire attack speed
    [SerializeField] private float spraySpeed;

    private void Awake()
    {
        fires = new Queue<GameObject>();

        for(int i = 0; i < 10; i++)
        {
            //Instantiate fire attack gameobject
            GameObject fire = Instantiate(firePrefab, transform.position, Quaternion.identity);
            //Disable newly instantiated fire attack gameobject
            fire.SetActive(false);
            //Add to gameobject pool
            fires.Enqueue(fire);
        }
    }

    private void OnEnable()
    {
        //Random number of fire attacks ranging from 5 to 10
        counts = Random.Range(5, 11);
        canAttack = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Continue spraying fire
        if (counts > 0)
        {
            if (canAttack)
            {
                StartCoroutine(ImpactTime(spraySpeed));
            }
        }
        //Stop fire cannon
        else
        {
            //Disable fire cannon
            gameObject.SetActive(false);
            //Boss fire cannon attack animation ends
            animator.SetBool("attack", false);
        }
    }

    IEnumerator ImpactTime(float seconds)
    {
        canAttack = false;
        Vector3 targetPos = character.transform.position;
        yield return new WaitForSeconds(seconds);
        //Decrease fire attack count left by 1
        counts--;
        //Start boss fire cannon attack animation
        animator.SetTrigger("fire");
        //Spawn fire attack gameobject from object pool
        GameObject fire = fires.Dequeue();
        //Fire attack appears at player's last position
        fire.transform.position = targetPos;
        fire.SetActive(true);
        fires.Enqueue(fire);
        canAttack = true;
    }
}
