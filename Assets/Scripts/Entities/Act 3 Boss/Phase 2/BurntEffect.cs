using UnityEngine;
using System.Collections;

public class BurntEffect : MonoBehaviour
{
    private bool burning;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private int damage;

    private void OnEnable()
    {
        burning = false;
        StartCoroutine(StatusDuration(3f));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!burning)
        {
            StartCoroutine(DamagePerSecond(damage));
        }
    }

    IEnumerator StatusDuration(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }

    IEnumerator DamagePerSecond(int dps)
    {
        burning = true;
        characterStats.health -= dps;
        yield return new WaitForSeconds(1f);
        burning = false;
    }
}
