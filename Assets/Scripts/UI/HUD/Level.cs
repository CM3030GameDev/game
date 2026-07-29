using UnityEngine;
using TMPro;

public class Level : MonoBehaviour
{
    private TMP_Text value;
    [SerializeField] private CharacterStats characterStats;

    private void Awake()
    {
        value = GetComponent<TMP_Text>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        value.text = $"{characterStats.level}";
    }
}
