using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelectionBar : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [SerializeField] private GameObject selectionBar;

    private void Start()
    {
        // Hide the cyan bar when the menu first opens
        if (selectionBar != null)
            selectionBar.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Show cyan bar
        if (selectionBar != null)
            selectionBar.SetActive(true);

        // Play your existing hover sound
        if (UIAudioManager.Instance != null)
            UIAudioManager.Instance.PlayHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hide cyan bar
        if (selectionBar != null)
            selectionBar.SetActive(false);
    }
}