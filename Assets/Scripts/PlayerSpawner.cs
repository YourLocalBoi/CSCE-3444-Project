using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    public Transform player;
    void Start()
    {
     if (PlayerPrefs.HasKey("ReturnX"))
        {
            float x = PlayerPrefs.GetFloat("ReturnX");
            float y = PlayerPrefs.GetFloat("ReturnY");

            player.position = new Vector3(x, y, 0);
        }   
    }
}
