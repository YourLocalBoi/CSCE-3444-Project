using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;

    public int maxHealth;
    public int currHealth;

    public int damage;

    public int defense;
    public bool isDefending = false;

    public bool TakeDamage(int damage)
    {

        if (isDefending) // Possibility to implement "armor crunch" so that if an attack reduces defense below 0, they take more damage
        {
            currHealth -= damage - ((defense * 2) + 1); // if the player or an enemy has chose to defend then it protects using 2x their defense for that turn (min 1 if defense is 0)
            isDefending = false;
        }
        else
        {
            currHealth -= damage - defense;
        }

        if (currHealth <= 0)
        {
            return true; // returns state of unit; isDead?
        }
        return false;
    }
}
