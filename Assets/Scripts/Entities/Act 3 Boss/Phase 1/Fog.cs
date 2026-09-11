using UnityEngine;

public class Fog : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color fogColor;
    //Fog appear timer
    private float appearTimer;
    //Fog disappear timer
    private float disappearTimer;
    [SerializeField] private GameObject fogs;
    [SerializeField] private float duration;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        fogColor = sr.color;
        appearTimer = 0f;
    }

    private void OnEnable()
    {
        //Set fog to be transparent at start
        fogColor.a = 0f;
        sr.color = fogColor;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Fog subside
        if (appearTimer > duration)
        {
            disappearTimer += Time.deltaTime;

            //Decrease alpha value until its fully transparent
            if (fogColor.a >= 0f)
            {
                float currentAlpha = Mathf.Lerp(0f, 1f, 5f - (disappearTimer / 3f));
                fogColor.a = currentAlpha;
                sr.color = fogColor;
            }
            //Disable fog when its fully transparent
            else
            {
                fogs.SetActive(false);
            }
        }
        //Fog slowly appear and stay
        else
        {
            appearTimer += Time.deltaTime;

            //Increment alpha value value until its fully opaque
            if (fogColor.a <= 1f)
            {
                float currentAlpha = Mathf.Lerp(0f, 1f, appearTimer / 3f);
                fogColor.a = currentAlpha;
                sr.color = fogColor;
            }
        }
    }
}
