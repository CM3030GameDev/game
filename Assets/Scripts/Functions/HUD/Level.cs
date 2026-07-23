using UnityEngine;
using TMPro;

public class Level : MonoBehaviour
{
    [SerializeField] private TMP_Text value;
    [SerializeField] private CompanionSystem companionSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        value.text = $"{companionSystem.characterLevel}";
    }
}
