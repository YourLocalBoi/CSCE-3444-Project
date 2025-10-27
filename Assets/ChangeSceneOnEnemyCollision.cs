using UnityEngine;
using UnityEngine.SceneManagement;  // Needed to change scenes

public class ChangeSceneOnEnemyCollision : MonoBehaviour
{
    // Name of the scene to load on collision
    public string sceneToLoad;

    // Using OnTriggerEnter if colliders have IsTrigger enabled
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    // Alternatively, if you're using regular colliders (no IsTrigger), use OnCollisionEnter:
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
    
}

