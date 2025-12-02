using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryUI : MonoBehaviour
{
    public float delayBeforeAdvance = 3f;   // seconds before moving on
    public string nextSceneName = "MainMenu";

    void Start()
    {
        // Show the victory UI immediately on enable
        gameObject.SetActive(true);

        // Begin auto-advance
        Invoke(nameof(AdvanceScene), delayBeforeAdvance);
    }

    void AdvanceScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
