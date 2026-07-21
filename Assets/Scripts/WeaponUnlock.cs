using UnityEngine;

public class WeaponUnlock : MonoBehaviour
{
    [SerializeField] private bool shotgun;
    [SerializeField] private bool laserbeam;
    [SerializeField] private bool bazooka;
    [SerializeField] private WeaponSystem weaponSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            Destroy(gameObject);
            if (shotgun)
            {
                weaponSystem.unlockedWeapons.Add("shotgun");
            }
            else if (laserbeam)
            {
                weaponSystem.unlockedWeapons.Add("laserbeam");
            }
            else if (bazooka)
            {
                weaponSystem.unlockedWeapons.Add("bazooka");
            }
        }
    }
}
