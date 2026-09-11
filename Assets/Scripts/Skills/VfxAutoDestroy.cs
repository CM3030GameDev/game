using UnityEngine;

// Cleans up a one shot visual effect after its animation, since nothing else disposes of it.
public class VfxAutoDestroy : MonoBehaviour
{
    [SerializeField] private float lifetime = 1f;

    private void Start() => Destroy(gameObject, lifetime);
}
