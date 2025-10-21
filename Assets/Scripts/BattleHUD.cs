using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public TextMeshProUGUI name;
    public Slider health;
    public TextMeshProUGUI currHealth;
    public TextMeshProUGUI maxHealth;

    public void SetHUD(Unit unit)
    {
        this.name.text = unit.unitName;
        health.maxValue = unit.maxHealth;
        health.value = unit.currHealth;
        this.currHealth.text = $"{unit.currHealth}";
        this.maxHealth.text = $"{unit.maxHealth}";
    }

    public void SetHealth(int health)
    {
        this.health.value = health;
        this.currHealth.text = health.ToString();
    }
}
