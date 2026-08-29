using UnityEngine;

public class TempExpSpawn : MonoBehaviour
{
    [SerializeField] private GameObject expOrbPrefab;
    [SerializeField] private Camera cam;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            Instantiate(expOrbPrefab, mouseWorldPos, Quaternion.identity);
        }
    }
}