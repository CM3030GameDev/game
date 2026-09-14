using UnityEngine;

public class RobotMob : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            collision.gameObject.GetComponent<Character>().CharacterAttacked(20, 0.5f);
        }
    }
}
