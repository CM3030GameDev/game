using UnityEngine;
using UnityEngine.UI;

public class Exp : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private CharacterStats characterStats;
    [SerializeField] private Progression progression;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        slider.maxValue = progression.ExpPerLevel;
        slider.value = characterStats.expPoint;
    }

    private void Update()
    {
        slider.maxValue = progression.ExpPerLevel;
        slider.value = characterStats.expPoint;
    }
}