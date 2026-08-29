using UnityEngine;
using UnityEngine.Events;

public class Act1ObjectiveTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent onPlayerEnter;
    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered || !other.CompareTag("Character")) return;
        triggered = true;
        onPlayerEnter?.Invoke();
    }
}
