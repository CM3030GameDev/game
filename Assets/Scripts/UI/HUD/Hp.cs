using UnityEngine;
using UnityEngine.UI;

public class Hp : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private SceneState sceneState;

    // Update is called once per frame
    void Update()
    {
        if (characterStats.health <= 0)
        {
            characterStats.health = 0;
            sceneState.dead = true;
        }

        fillImage.fillAmount = (float)characterStats.health / characterStats.maxHealth;
    }
}
