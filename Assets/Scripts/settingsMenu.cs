using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsMenu : MonoBehaviour
{
    [Header("References")]
    public PauseMenuInGame pauseMenu;  
    public EventSystem eventSystem;

    [Header("UI Navigation Order")]
    public List<Selectable> navigationItems = new List<Selectable>();

    private int currentIndex = 0;

    private void Awake()
    {
        if (pauseMenu == null)
            pauseMenu = FindFirstObjectByType<PauseMenuInGame>();

        if (eventSystem == null)
            eventSystem = FindFirstObjectByType<EventSystem>();
    }

    private void OnEnable()
    {
        if (navigationItems.Count > 0)
        {
            currentIndex = 0;
            eventSystem.SetSelectedGameObject(navigationItems[currentIndex].gameObject);
        }
    }

    private void Update()
    {
        if (!gameObject.activeInHierarchy)
            return;

        HandleNavigation();
    }

    private void HandleNavigation()
    {
        if (navigationItems.Count == 0) return;

        // Move down
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            MoveSelection(1);

        // Move up
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            MoveSelection(-1);

        // ─── Back Button ────────────────────────────────
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("Cancel"))
        {
            BackToPauseMenu();
        }
    }

    private void MoveSelection(int direction)
    {
        currentIndex += direction;

        if (currentIndex >= navigationItems.Count)
            currentIndex = 0;

        if (currentIndex < 0)
            currentIndex = navigationItems.Count - 1;

        Selectable selected = navigationItems[currentIndex];
        eventSystem.SetSelectedGameObject(selected.gameObject);
    }

    // ────────────────────────────────────────────────
    // CALLBACK FOR THE BACK BUTTON IN THE UI
    // ────────────────────────────────────────────────
    public void BackToPauseMenu()
    {
        if (pauseMenu != null)
        {
            gameObject.SetActive(false);
            pauseMenu.BackToPauseMenu();
        }
        else
        {
            Debug.LogError("PauseMenuInGame reference missing in SettingsMenu!");
        }
    }
}
