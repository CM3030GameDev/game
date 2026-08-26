using UnityEngine;

// LoL-style ability pips: each pip is a black (empty) piece with a white (filled) piece
// stacked on top of it, pre-sprited in the Inspector. This just toggles the filled
// piece on/off per pip - it never touches sprites at runtime, so it works whether the
// 3 sliced pieces are identical or visually distinct (e.g. left/middle/right shaped).
public class LevelPips : MonoBehaviour
{
    [SerializeField] private GameObject[] filledPips;

    private void Awake() => SetLevel(0);

    public void SetLevel(int level)
    {
        for (int i = 0; i < filledPips.Length; i++)
            filledPips[i].SetActive(i < level);
    }
}
