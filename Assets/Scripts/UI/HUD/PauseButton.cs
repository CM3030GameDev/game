using UnityEngine;
using UnityEngine.UI;

// HUD pause button that finds the scene's Interface on click, since the HUD prefab cannot reference scene objects
[RequireComponent(typeof(Button))]
public class PauseButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Interface ui = FindAnyObjectByType<Interface>();
        if (ui != null) ui.TogglePause();
    }
}
