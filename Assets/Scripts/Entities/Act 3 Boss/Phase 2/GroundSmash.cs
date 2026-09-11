using UnityEngine;
using System.Collections;

public class GroundSmash : MonoBehaviour
{
    //Ground impact attacks left
    private int impactCount;
    //Attack state
    private bool canAttack;
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private GameObject impactPrefab;
    //Interval between impacts
    [SerializeField] private float interval;

    private void OnEnable()
    {
        //Random number of impact attack ranging from 20 to 30
        impactCount = Random.Range(20, 31);
        canAttack = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Continue ground smash attack
        if(impactCount > 0)
        {
            if (canAttack)
            {
                StartCoroutine(ImpactTime(interval));
            }
        }
        //Ground smash attack ends
        else
        {
            //Disable ground smash attack
            gameObject.SetActive(false);
            //Boss smash attack animation ends
            bossAnimator.SetBool("smash", false);
        }
    }

    IEnumerator ImpactTime(float seconds)
    {
        canAttack = false;

        Vector3 targetPos;
        //Aim at player most recent position
        if (Random.Range(0f, 1f) < 0.5f)
        {
            targetPos = PhaseTwoManager.Instance.characterTransform.position;
        }
        //Aim at player next predicted position
        else
        {
            targetPos = PhaseTwoManager.Instance.characterTransform.position + (Vector3)PhaseTwoManager.Instance.character.MoveInput;
        }

        //Get indicator gameobject from object pool
        GameObject indicator = PhaseTwoManager.Instance.indicators.Dequeue();
        //Set indicator to mark targeted position
        indicator.transform.position = targetPos;
        //Indicator appears at targeted position
        indicator.SetActive(true);
        yield return new WaitForSeconds(seconds);
        //Decrease impact attack count left by 1
        impactCount--;
        //Remove indicator
        indicator.SetActive(false);
        //Return indicator back to object pool
        PhaseTwoManager.Instance.indicators.Enqueue(indicator);
        //Spawn ground impact gameobject from object pool
        GameObject impact = PhaseTwoManager.Instance.impacts.Dequeue();
        //Ground impact appears at player's last position
        impact.transform.position = targetPos;
        //Impact appears at targeted position
        impact.SetActive(true);
        canAttack = true;
    }
}
