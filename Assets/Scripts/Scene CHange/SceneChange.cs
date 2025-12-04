using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string sceneToLoad;
    public Animator trans;
    public float fadeTime = 1f;
    public string enemyID = "";
    public Transform exitLocation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerPrefs.SetInt(enemyID, 1);   // MARK ENEMY AS DEAD
            
            PlayerPrefs.SetFloat("ReturnX", exitLocation.position.x);
            PlayerPrefs.SetFloat("ReturnY", exitLocation.position.y);

            PlayerPrefs.Save();

            trans.Play("FirstTransition");
            StartCoroutine(DelayFade());
        }
    }

    IEnumerator DelayFade()
    {
        yield return new WaitForSeconds(fadeTime);

        SceneManager.LoadScene(sceneToLoad);
    }
}
