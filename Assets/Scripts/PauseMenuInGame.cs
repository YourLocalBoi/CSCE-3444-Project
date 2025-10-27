using System.Collections.Generic;
using UnityEngine;
using TMPro;  

/// <summary>
/// - Opens/closes with Escape
/// - Pauses the game when open (Time.timeScale = 0)
/// - Navigate with W/S or Up/Down arrows
/// - Confirm with Enter or Space
/// - Back (close) with Escape
/// </summary>
public class PauseMenuInGame : MonoBehaviour
{
    // UI References

    [Header("Root list labels (use TextMeshProUGUI objects)")]
    [SerializeField] private List<TMP_Text> labels = new List<TMP_Text>();
    //on-screen text options like "Inventory", "Stats", "Settings"

    [Header("Panels that open when options are selected")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject settingsPanel;

  

    [Header("Look & Feel")]
    [SerializeField] private Color normalColor = new Color(1f, 1f, 1f, 0.7f);  // non-selected color
    [SerializeField] private Color selectedColor = Color.white;                // highlight color
    [SerializeField] private float selectedScale = 1.06f;                      // how much bigger the selected label is
    [SerializeField] private float navCooldownTime = 0.15f;                    // how fast you can scroll (seconds between moves)

   

    private bool isOpen = false;    // Is the pause menu currently visible?
    private bool inSubPanel = false; // Are we currently inside Inventory/Stats/Settings panel?
    private int selected = 0;       // Which menu item is highlighted
    private float navCooldown = 0f; // Timer to control how fast you can move between options

    // List of all content panels for easy hiding
    private List<GameObject> allPanels = new List<GameObject>();

    // INITIALIZATION
    void Awake()
    {
        // Collect panels into a single list for easy iteration
        if (inventoryPanel) allPanels.Add(inventoryPanel);
        if (statsPanel) allPanels.Add(statsPanel);
        if (settingsPanel) allPanels.Add(settingsPanel);

        // Make sure all panels start hidden
        foreach (var p in allPanels)
            if (p) p.SetActive(false);

        // Hide the menu entirely until Escape is pressed
        SetMenuVisible(false);
    }

    void Update()
    {
        // The key used to open/close the menu
        bool escapePressed = Input.GetKeyDown(KeyCode.Escape);

  
        if (!isOpen)
        {
            if (escapePressed)
                OpenMenu();
            return;
        }

        // If we are in a sub-panel (Inventory/Stats/Settings)
        // pressing Escape should close the current panel instead of closing the whole menu
        if (inSubPanel)
        {
            if (escapePressed)
                CloseAllPanels();
            return; // don't allow navigation while a panel is open
        }

        // Reduce cooldown timer (so you can't move too fast through the list)
        navCooldown -= Time.unscaledDeltaTime;

        //Read keyboard input 
        bool moveUp = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
        bool moveDown = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);
        bool confirm = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space);

        //Move selection if cooldown allows
        if (navCooldown <= 0f)
        {
            if (moveUp)
            {
                MoveSelection(-1);
                navCooldown = navCooldownTime; // reset delay
            }
            else if (moveDown)
            {
                MoveSelection(+1);
                navCooldown = navCooldownTime;
            }
        }

       
        if (confirm)
        {
            ActivateSelected();
        }

        //
        if (escapePressed)
        {
            CloseMenu();
        }
    }
    // MENU CONTROL FUNCTIONS

    // Opens the pause menu and freezes game time
    void OpenMenu()
    {
        isOpen = true;
        inSubPanel = false;

        // Make sure selection index is valid
        selected = Mathf.Clamp(selected, 0, labels.Count - 1);

        // Show the UI
        SetMenuVisible(true);

        // Highlight current selection
        RefreshVisuals();

        // Pause the game
        Time.timeScale = 0f;
    }

    // Closes the pause menu and resumes game time
    void CloseMenu()
    {
        CloseAllPanels();
        isOpen = false;
        SetMenuVisible(false);
        Time.timeScale = 1f; // resume game
    }

    // Hides all panels (Inventory, Stats, Settings)
    void CloseAllPanels()
    {
        foreach (var p in allPanels)
            if (p) p.SetActive(false);

        inSubPanel = false;
    }

    // Show a specific panel
    void OpenPanel(GameObject panel)
    {
        if (!panel) return;
        CloseAllPanels();        // close any others
        panel.SetActive(true);   // show the chosen one
        inSubPanel = true;       
    }

    // Shows or hides the menu UI (without affecting panels)
    void SetMenuVisible(bool visible)
    {
        // Enable/disable all children of this GameObject (the UI)
        foreach (Transform child in transform)
            child.gameObject.SetActive(visible);
    }

    // SELECTION + VISUALS

    // Moves the current selection index
    void MoveSelection(int direction)
    {
        if (labels.Count == 0) return;

        selected = (selected + direction + labels.Count) % labels.Count;
        RefreshVisuals();
    }

    // Called when the user presses Enter/Space on a menu item
    void ActivateSelected()
    {
        switch (selected)
        {
            case 0: // Inventory
                OpenPanel(inventoryPanel);
                break;

            case 1: // Stats
                OpenPanel(statsPanel);
                break;

            case 2: // Settings
                OpenPanel(settingsPanel);
                break;
        }
    }

    // Updates label colors/scales to highlight the selected one
    void RefreshVisuals()
    {
        for (int i = 0; i < labels.Count; i++)
        {
            bool isSelected = (i == selected);

            // Safety check: skip null labels
            if (!labels[i]) continue;

            // Set color
            labels[i].color = isSelected ? selectedColor : normalColor;

            // Set font weight
            labels[i].fontStyle = isSelected ? FontStyles.Bold : FontStyles.Normal;

            // Set slight scale change
            labels[i].transform.localScale = isSelected ? Vector3.one * selectedScale : Vector3.one;
        }
    }
}
