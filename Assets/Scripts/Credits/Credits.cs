using UnityEngine;

public class Credits : MonoBehaviour
{
    private RectTransform text;
    [SerializeField] private float scrollSpeed = 50f;

    private void Awake()
    {
        text = GetComponent<RectTransform>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Keep scrolling until the last text's y position
        if(text.localPosition.y < 500)
        {
            text.localPosition += new Vector3(0, scrollSpeed * Time.deltaTime, 0);
        }
    }
}
