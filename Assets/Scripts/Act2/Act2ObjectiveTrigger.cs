using UnityEngine;
using UnityEngine.Events;

public class Act2ObjectiveTrigger : MonoBehaviour
{
    public GameObject gateObject;

    private PolygonCollider2D triggerCollider;
    private bool isGateOpen = false;
    private bool isGateTriggered = false;

    void Awake()
    {
        triggerCollider = GetComponent<PolygonCollider2D>();

        if (gateObject != null)
        {
            gateObject.SetActive(true);
        }
    }

    public void OpenGate()
    {
        if (gateObject != null && !isGateOpen)
        {
            gateObject.SetActive(false);
            isGateOpen = true;
        }
    }

    private void CloseGate()
    {
        if (gateObject != null && isGateOpen)
        {
            gateObject.SetActive(true);
            isGateOpen = false;
        }

        if (triggerCollider != null)
        {
            triggerCollider.enabled = false;
        }
    }

    public Transform GetGateTransform()
    {
        if (gateObject == null)
            return gateObject.transform;
        else
            return null;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isGateOpen && other.CompareTag("Character"))
        {
            CloseGate();
            isGateTriggered = true;
        }
    }

    public bool GetIsGateTriggered()
    {
        return isGateTriggered;
    }
}
