using UnityEngine;
using System.Collections;

public class ConfusionEffect : MonoBehaviour
{
    [SerializeField] private Character character;

    private void OnEnable()
    {
        //Update character confusion status
        character.confused = true;
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

    IEnumerator StatusDuration(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        //Update character confusion status
        character.confused = false;
        gameObject.SetActive(false);
    }
}
