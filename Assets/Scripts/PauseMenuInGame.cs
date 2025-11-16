using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenuInGame : MonoBehaviour
{
    // UI References
    [Header("Root list labels (use TextMeshProUGUI objects)")]
    [SerializeField] private List<TMP_Text> labels = new List<TMP_Text>();

    [Header("Panels that open when options are selected")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private GameObject settingsPanel;

    [Header("Look & Feel")]
    [SerializeField] private Color normalColor = new Color(1f, 1f, 1f, 0.7f);
    [SerializeField] private Color selectedColor = Color.white;
    [SerializeField] private float selectedScale = 1.06f;
    [SerializeField] private float navCooldownTime = 0.15f;

    private bool isOpen = false;
    private bool inSubPanel = false;
    private int selected = 0;
    private float navCooldown = 0f;

    private List<GameObject> allPanels = new List<GameObject>();

    // -------------------------------------------------------
    // INITIALIZATION
    // -------------------------------------------------------
    void Awake()
    {
        if (inventoryPanel) allPanels.Add(inventoryPanel);
        if (statsPanel) allPanels.Add(statsPanel);
        if (settingsPanel) allPanels.Add(settingsPanel);

        foreach (var p in allPanels)
            if (p) p.SetActive(false);

        SetMenuVisible(false);
    }

    // -------------------------------------------------------
    // UPDATE LOOP
    // -------------------------------------------------------
    void Update()
    {
        bool escapePressed = Input.GetKeyDown(KeyCode.Escape);

        if (!isOpen)
        {
            if (escapePressed)
                OpenMenu();
            return;
        }

        // If inside a sub-panel, ESC closes the panel, NOT the entire menu
        if (inSubPanel)
        {
            if (escapePressed)
                BackToPauseMenu();  // <-- unified return function
            return;
        }

        // Navigation cooldown
        navCooldown -= Time.unscaledDeltaTime;

        bool moveUp = Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
        bool moveDown = Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);
        bool confirm = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space);

        if (navCooldown <= 0f)
        {
            if (moveUp)
            {
                MoveSelection(-1);
                navCooldown = navCooldownTime;
            }
            else if (moveDown)
            {
                MoveSelection(+1);
                navCooldown = navCooldownTime;
            }
        }

        if (confirm)
            ActivateSelected();

        if (escapePressed)
            CloseMenu();
    }

    // -------------------------------------------------------
    // MENU CONTROL
    // -------------------------------------------------------
    void OpenMenu()
    {
        isOpen = true;
        inSubPanel = false;

        selected = Mathf.Clamp(selected, 0, labels.Count - 1);

        SetMenuVisible(true);
        RefreshVisuals();
        Time.timeScale = 0f;
    }

    void CloseMenu()
    {
        CloseAllPanels();
        isOpen = false;
        SetMenuVisible(false);
        Time.timeScale = 1f;
    }

    void SetMenuVisible(bool visible)
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(visible);
    }

    // -------------------------------------------------------
    // PANEL CONTROL
    // -------------------------------------------------------
    void CloseAllPanels()
    {
        foreach (var p in allPanels)
            if (p) p.SetActive(false);
    }

    void OpenPanel(GameObject panel)
    {
        if (!panel) return;

        CloseAllPanels();

        // Hide the MAIN pause menu UI when entering a sub-panel
        SetMenuVisible(false);

        panel.SetActive(true);
        inSubPanel = true;
    }

    // -------------------------------------------------------
    // PUBLIC BACK FUNCTION (for SettingsMenu.cs)
    // -------------------------------------------------------
    public void BackToPauseMenu()
    {
        CloseAllPanels();  
        inSubPanel = false;

        // Re-open pause menu UI
        SetMenuVisible(true);
        RefreshVisuals();
    }

    // -------------------------------------------------------
    // NAVIGATION + ACTIVATION
    // -------------------------------------------------------
    void MoveSelection(int direction)
    {
        if (labels.Count == 0) return;

        selected = (selected + direction + labels.Count) % labels.Count;
        RefreshVisuals();
    }

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

    // -------------------------------------------------------
    // VISUALS
    // -------------------------------------------------------
    void RefreshVisuals()
    {
        for (int i = 0; i < labels.Count; i++)
        {
            bool isSelected = (i == selected);

            if (!labels[i]) continue;

            labels[i].color = isSelected ? selectedColor : normalColor;
            labels[i].fontStyle = isSelected ? FontStyles.Bold : FontStyles.Normal;
            labels[i].transform.localScale = isSelected ? Vector3.one * selectedScale : Vector3.one;
        }
    }
}
