using UnityEngine;

public class EnemyParentDisabler : MonoBehaviour
{
    public string enemyID = "";

    private void Awake()
    {
        if (PlayerPrefs.GetInt(enemyID, 0) == 1)
        {
            gameObject.SetActive(false);
        }
    }
}

