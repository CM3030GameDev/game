using UnityEngine;

public class SpawnAirBlast : MonoBehaviour
{
    [SerializeField] private GameObject airblastPrefab;
    [SerializeField] private float airblastSpeed;

    private void OnEnable()
    {
        GameObject airBlast = Instantiate(airblastPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = airBlast.GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * airblastSpeed;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
