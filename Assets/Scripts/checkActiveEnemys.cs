using UnityEngine;

public class DetectThreeDisabled : MonoBehaviour
{
    [Header("Objects to watch (must become disabled)")]
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;

    [Header("What to enable when all 3 are disabled")]
    public GameObject objectToEnable;

    [Header("What to disable when all 3 are disabled")]
    public GameObject objectToDisable;

    private bool actionTriggered = false;

    void Update()
    {
        // Only run once
        if (actionTriggered)
            return;

        // Check if all 3 objects are disabled
        if (!object1.activeSelf && !object2.activeSelf && !object3.activeSelf)
        {
            TriggerActions();
        }
    }

    void TriggerActions()
    {
        actionTriggered = true;

        if (objectToEnable != null)
            objectToEnable.SetActive(true);

        if (objectToDisable != null)
            objectToDisable.SetActive(false);
    }
}
