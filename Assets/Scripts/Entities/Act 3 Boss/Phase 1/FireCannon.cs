using UnityEngine;
using System.Collections;

public class FireCannon : MonoBehaviour
{
    //Number of explosions left
    private int counts;
    //Check if cannon can fire again
    private bool canFire;
    [SerializeField] private FinalBossOne finalBossOne;
    [SerializeField] private Animator animator;
    //Fire attack speed
    [SerializeField] private float spraySpeed;

    private void OnEnable()
    {
        //Random number of fire attacks ranging from 10 to 20
        counts = Random.Range(10, 21);
        canFire = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(finalBossOne.death)
        {

        }

        //Continue spraying fire
        if (counts > 0)
        {
            if (canFire)
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
            //Update boss attack state
            finalBossOne.attacking = false;
            //Refresh next attack timer
            finalBossOne.attackTime = 0f;
            //Refresh next random attack
            finalBossOne.randomNum = Random.Range(0f, 1f);
        }
    }

    IEnumerator ImpactTime(float seconds)
    {
        canFire = false;

        Vector3 targetPos;
        //Aim at player most recent position
        if (Random.Range(0f, 1f) < 0.5f)
        {
            targetPos = PhaseOneManager.Instance.characterTransform.position;
        }
        //Aim at player next predicted position
        else
        {
            targetPos = PhaseOneManager.Instance.characterTransform.position + ((Vector3)PhaseOneManager.Instance.character.MoveInput * 2);
        }

        //Show hitbox indicator at targeted position;
        GameObject indicator = PhaseOneManager.Instance.indicators.Dequeue();
        indicator.transform.position = targetPos;
        indicator.SetActive(true);
        yield return new WaitForSeconds(seconds);
        //Decrease explosion count left by 1
        counts--;
        //Start boss fire cannon attack animation
        animator.SetTrigger("fire");
        //Disable hitbox indicator
        indicator.SetActive(false);
        //Return indicator object back to object pool
        PhaseOneManager.Instance.indicators.Enqueue(indicator);
        //Spawn explosion gameobject from object pool
        GameObject explosion = PhaseOneManager.Instance.explosions.Dequeue();
        //Explosion appears at player's last position
        explosion.transform.position = targetPos;
        //Start firing explosion
        explosion.SetActive(true);
        //Play fire explosion sound effect
        UIAudioManager.Instance.PlaySFXOneShot(2, 0.5f);
        //Update fire state
        canFire = true;
    }
}
