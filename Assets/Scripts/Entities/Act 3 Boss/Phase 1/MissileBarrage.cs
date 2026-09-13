using System.Collections;
using UnityEngine;

public class MissileBarrage : MonoBehaviour
{
    //Number of missiles left to shoot
    private int missileCount;
    //Shooting state
    private bool shooting;
    private GameObject missile;
    [SerializeField] private FinalBossOne finalBossOne;
    [SerializeField] private Animator animator;
    [SerializeField] private float interval;

    private void OnEnable()
    {
        //Shoot between 30 to 40 missiles
        missileCount = Random.Range(30, 41);
        shooting = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(missileCount > 0)
        {
            if(!shooting)
            {
                StartCoroutine(MissileTime(interval));
            }
        }
        else
        {
            finalBossOne.attacking = false;
            finalBossOne.randomNum = Random.Range(0f, 1f);
            finalBossOne.attackTime = 0f;
            //End boss missile attack animation
            animator.SetBool("attack", false);
            //Stop missile barrage attack
            gameObject.SetActive(false);
        }
    }

    //Interval between firing missile towards sky
    IEnumerator MissileTime(float seconds)
    {
        shooting = true;
        //Decrease missile count left by 1
        missileCount--;
        missile = PhaseOneManager.Instance.missiles.Dequeue();
        //Fire missile from current position
        missile.transform.position = transform.position;
        //Angle it at 90 degree upwards towards the sky
        missile.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        //Start shooting missile
        missile.SetActive(true);
        //Play missile sound effect
        UIAudioManager.Instance.PlaySFXOneShot(1, 0.3f);
        Rigidbody2D rb = missile.GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * 10f;
        yield return new WaitForSeconds(seconds);
        shooting = false;
    }
}
