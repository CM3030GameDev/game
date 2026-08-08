using UnityEngine;

public class CameraDrift : MonoBehaviour
{
    [SerializeField] private float driftAmount = 0.15f;
    [SerializeField] private float driftSpeed = 0.25f;

    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float x = Mathf.Sin(Time.time * driftSpeed) * driftAmount;

        float y = Mathf.Cos(Time.time * driftSpeed * 0.7f) * (driftAmount * 0.5f);

        transform.position = startPosition + new Vector3(x, y, 0);
    }
}