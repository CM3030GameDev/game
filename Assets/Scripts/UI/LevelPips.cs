using UnityEngine;
using UnityEngine.UI;

// LoL-style ability pips: a row of dots under a slot icon, filled left-to-right as it levels up.
public class LevelPips : MonoBehaviour
{
    [SerializeField] private Image[] pips;
    [SerializeField] private Color filledColor = Color.white;
    [SerializeField] private Color emptyColor = Color.black;

    public void SetLevel(int level)
    {
        for (int i = 0; i < pips.Length; i++)
            pips[i].color = (i < level) ? filledColor : emptyColor;
    }
}
