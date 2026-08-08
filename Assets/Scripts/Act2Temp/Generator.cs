using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    [Header("Generator Shield Object")]
    [SerializeField] private GameObject generatorShield;

    [Header("References")]
    public Act2Manager act2Manager;

    private bool isShieldDown = false;
    private bool isOverdrive = false;
    private bool isGeneratorDown = false;

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
        generatorHealth = maxGeneratorHealth;
        generatorEnergy = maxGeneratorEnergy;

        canvasUI.SetActive(false);
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
            act2Manager.OnGeneratorShieldDown();
            DisableShield();
        }

        if (generatorHealth <= 0 && isGeneratorDown == false)
        {
            isGeneratorDown = true;
            UnlockNextAct();
        }
    }
    public void DisableShield() //killing the boss disables the shield
    {
        isShieldDown = true;
        isOverdrive = false;
        generatorShield.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isShieldDown && generatorEnergy <= 0 && collision.gameObject.CompareTag("Bullet"))
        {
            generatorHealth -= 1;
        }
    }
    public void UnlockNextAct()
    {
        act2Manager.OnGeneratorDown();
    }
    public void EnhancedShield()
    {
        //change shield color
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
