using UnityEngine;
using UnityEngine.UI;

public class Hp : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private SceneState sceneState;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.value = characterStats.health;
        slider.maxValue = characterStats.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        slider.maxValue = characterStats.maxHealth;
        if(characterStats.health <= 0)
        {
            characterStats.health = 0;
            sceneState.dead = true;
        }
        else
        {
            slider.value = characterStats.health;
        }
    }
}
