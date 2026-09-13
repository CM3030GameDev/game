using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossUltimate : MonoBehaviour
{
    public Animator bossAnimator;

    [Header("Ult Settings")]
    [SerializeField] private GameObject ult;
    [SerializeField] private Transform topPoint;
    [SerializeField] private int numberOfThrows = 3;
    [SerializeField] private float chargingTime = 1f;
    [SerializeField] private float maxScale = 5f;
    [SerializeField] private float throwSpeed = 8f;
    [SerializeField] private float ultLifetime = 5f;
    [SerializeField] private int ultDamage = 10;
    [SerializeField] private float playerInvulnerability = 0.1f;

    private bool isUltStart = false;
    private bool isUltEnd = false;
    private Transform playerTransform;
    private string facingDir = "F";
    private Act2Miniboss boss;
    private GameObject ultObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isUltStart = false;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Character");
        if (playerObj != null)
            playerTransform = playerObj.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(isUltStart)
        {
            isUltStart = false;
            StartCoroutine(Ultimate());
        }
    }

    private IEnumerator Ultimate()
    {
        isUltEnd = false;

        if (bossAnimator != null)
            bossAnimator.Play(facingDir + "IdleCarrying");

        for (int i = 0; i < numberOfThrows; i++)
        {
            UIAudioManager.Instance.PlaySFX(1);
            ultObject = Instantiate(ult, topPoint.position, Quaternion.identity);
            ultObject.GetComponent<CircleCollider2D>().enabled = false;
            yield return StartCoroutine(GrowUlt(ultObject));
            UIAudioManager.Instance.StopSFX();
            ThrowUlt(ultObject);
        }
        isUltEnd = true;
    }

    private IEnumerator GrowUlt(GameObject ultObject)
    {
        ultObject.transform.localScale = Vector3.zero;

        float currentTime = 0f;
        while (currentTime < chargingTime)
        {
            currentTime += Time.deltaTime;
            float t = currentTime / chargingTime;
            ultObject.transform.localScale = Vector3.one * Mathf.Lerp(0f, maxScale, t);
            yield return null;
        }
        ultObject.transform.localScale = Vector3.one * maxScale;
    }

    private void ThrowUlt(GameObject instance)
    {
        if (playerTransform == null) return;

        Vector3 direction = (playerTransform.position - instance.transform.position).normalized;
        UltProjectile projectile = instance.GetComponent<UltProjectile>();
        if (projectile != null)
        {
            projectile.Launch(direction, throwSpeed, ultLifetime, ultDamage, playerInvulnerability);
            UIAudioManager.Instance.PlaySFXOneShot(2);
            projectile.GetComponent<CircleCollider2D>().enabled = true;
        }
        
    }

    public void SetIsStartUlt(bool startUlt, string direction = "F")
    {
        isUltStart = startUlt;
        facingDir = direction;
    }

    public void SetIsEndUlt(bool endUlt, Act2Miniboss miniboss)
    {
        isUltEnd = endUlt;
        boss = miniboss;
        boss.bossDeath.AddListener(BossDeath);
    }

    private void BossDeath()
    {
        StopAllCoroutines();
        Destroy(ultObject);
    }

    public bool GetIsUltEnd()
    {
        return isUltEnd;
    }
}
