using UnityEngine;

// Ability pips in the LoL style: an empty piece per level with a filled piece stacked on top.
// Only toggles the filled pieces on and off, it never swaps sprites at runtime.
public class LevelPips : MonoBehaviour
{
    [SerializeField] private GameObject[] filledPips;
    [Tooltip("One green cover per pip, shown once this weapon/stat has evolved. Parent each one " +
             "under its own Pip so the layout group never sees it. Safe to leave empty.")]
    [SerializeField] private GameObject[] combinedPips;

    private void Awake()
    {
        SetLevel(0);
        SetCombined(false);   // a fresh run always starts uncombined
    }

    public void SetLevel(int level)
    {
        for (int i = 0; i < filledPips.Length; i++)
            filledPips[i].SetActive(i < level);
    }

    public void SetCombined(bool combined)
    {
        if (combinedPips == null) return;
        foreach (GameObject pip in combinedPips)
            if (pip != null) pip.SetActive(combined);
    }
}
