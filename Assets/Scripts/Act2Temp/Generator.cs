using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    [Header("Generator Shield Object")]
    [SerializeField] private GameObject generatorShield;
    [SerializeField] private SpriteRenderer generatorShieldSprite;
    [SerializeField] private Animator shieldAnimator;

    [Header("Events")]
    public UnityEvent onGeneratorShieldDown;
    public UnityEvent onGeneratorDown;

    private bool isShieldDown = false;
    private bool isOverdrive = false;
    private bool isGeneratorDown = false;
    private bool isShieldEnhanced = false;
    StateTrigger exitTrigger;

    [Header("Generator Stats")]
    [SerializeField] private int maxGeneratorHealth = 5;
    [SerializeField] private float maxGeneratorEnergy = 100f;
    [SerializeField] private int generatorHealth;
    [SerializeField] private float generatorEnergy;

    [Header("UI Components")]
    [SerializeField] private Image generatorEnergyUI;
    [SerializeField] private Image generatorHealthUI;
    [SerializeField] private GameObject canvasUI;

    void Start()
    {
        generatorShield.SetActive(true);
        shieldAnimator.Play("GeneratorShieldBlue");
        generatorHealth = maxGeneratorHealth;
        generatorEnergy = maxGeneratorEnergy;

        canvasUI.SetActive(false);
        exitTrigger = shieldAnimator.GetBehaviour<StateTrigger>();
    }
    void Update()
    {
        if (canvasUI.activeSelf)
        {
            generatorEnergyUI.fillAmount = generatorEnergy / maxGeneratorEnergy;
            generatorHealthUI.fillAmount = (float)generatorHealth / maxGeneratorHealth;

            if (generatorHealth <= 0)
            {
                canvasUI.SetActive(false);
            }
        }

        if (isOverdrive && generatorEnergy > 0)
        {
            EnhancedShield();
            generatorEnergy -= Time.deltaTime;
        }
        else if (isOverdrive && generatorEnergy <= 0)
        {
            onGeneratorShieldDown?.Invoke();
            DisableShield();
        }

        if (generatorHealth <= 0 && isGeneratorDown == false)
        {
            isGeneratorDown = true;
            UnlockNextAct();
        }
    }
    private void DisableShield() //killing the boss disables the shield
    {
        if(!isShieldDown && isOverdrive)
        {
            isShieldDown = true;
            isOverdrive = false;
            shieldAnimator.Play("GeneratorShieldCollapse");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isShieldDown && generatorEnergy <= 0 && collision.gameObject.CompareTag("Bullet"))
        {
            generatorHealth -= 1;
        }
    }
    public void UnlockNextAct()
    {
        onGeneratorDown?.Invoke();
    }
    public void EnhancedShield()
    {
        if(!isShieldEnhanced)
        {
            isShieldEnhanced = true;
            shieldAnimator.Play("GeneratorShieldYellow");
            // 2. Fetch the generic exit behavior from that specific state
            

            if (exitTrigger != null)
            {
                // Clear any old subscriptions to prevent bugs
                exitTrigger.OnStateExitAction = null;

                // 3. Subscribe to the exit event
                exitTrigger.OnStateExitAction += OnAnimationEnd;
            }
        }
    }

    private void OnAnimationEnd()
    {
        generatorShield.SetActive(false);
        exitTrigger.OnStateExitAction = null;
    }

    public void SetIsOverdrive(bool overdrive)
    {
        //just in case other scripts keep calling setIsOverdrive(true) even when it's over
        if (generatorEnergy <= 0)
            return;

        isOverdrive = overdrive;
        canvasUI.SetActive(true);
    }

    //Testing purposes
    [ContextMenu("Debug: Trigger setIsOverdrive(True)")]
    public void DebugTriggerOverdrive()
    {
        SetIsOverdrive(true);
        Debug.Log("Inspector Test: Sent setIsOverdrive(true) to Generator");
    }
}

//-----sequence-----
//Note:
//enhancedShield() is just a visual thing for the shield
//isOverdrive means generator starts spawning mobs
//------------------
//1. boss dies -> call setIsOverdrive(true)
//2. in update, generatorEnergy starts depleting (spawn enemies now)
//3. in update, canvas is enabled from isOverdrive to when generatorHealth <= 0
//4. in update, generatorEnergy is depleted, calls disableShield()
//5. in colliderEnter, shield is down and energy is depleted, allow player to hit the generator
//6. in update, generatorHealth <= 0, calls unlockNextAct() once
