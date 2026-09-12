using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hp : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private SceneState sceneState;
    [Tooltip("Optional \"100 / 100\" readout centred in the bar.")]
    [SerializeField] private TMP_Text valueText;

    private int lastHealth = -1;
    private int lastMaxHealth = -1;

    // Update is called once per frame
    void Update()
    {
        // Clamp only. PlayerRespawn owns what actually happens when the player hits zero.
        if (characterStats.health <= 0) characterStats.health = 0;

        fillImage.fillAmount = (float)characterStats.health / characterStats.maxHealth;

        // Only rebuild the string when a number actually changed, since this runs every frame.
        if (valueText != null &&
            (characterStats.health != lastHealth || characterStats.maxHealth != lastMaxHealth))
        {
            lastHealth = characterStats.health;
            lastMaxHealth = characterStats.maxHealth;
            valueText.text = lastHealth + " / " + lastMaxHealth;
        }
    }
}
