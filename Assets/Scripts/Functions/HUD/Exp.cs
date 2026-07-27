using UnityEngine;
using UnityEngine.UI;

public class Exp : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private CharacterStats characterStats;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.value = characterStats.expPoint;
    }

    // Update is called once per frame
    void Update()
    {
        if (characterStats.expPoint >= 100)
        {
            characterStats.expPoint = characterStats.expPoint % 100;
            characterStats.level += 1;
        }
        else
        {
            slider.value = characterStats.expPoint;
        }
    }
}
