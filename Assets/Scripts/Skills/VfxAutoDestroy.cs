using UnityEngine;

// Cleans up a one-shot visual effect once its animation has played. Instantiated VFX have no
// owner to dispose them - the air strike spawns one per enemy, so ~30 per cast would otherwise
// accumulate in the scene forever.
public class VfxAutoDestroy : MonoBehaviour
{
    [SerializeField] private float lifetime = 1f;

    private void Start() => Destroy(gameObject, lifetime);
}
