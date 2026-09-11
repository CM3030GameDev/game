using UnityEngine;
using UnityEngine.Events;

public class Act1ObjectiveTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent onPlayerEnter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character")) onPlayerEnter?.Invoke();
    }
}
