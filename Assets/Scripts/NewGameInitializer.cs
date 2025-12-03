using UnityEngine;

public class NewGameInitializer : MonoBehaviour
{
    private static bool hasCleared = false;

    void Awake()
    {
        if (!hasCleared)
        {
            PlayerPrefs.DeleteAll();
            hasCleared = true;
        }
    }
}


