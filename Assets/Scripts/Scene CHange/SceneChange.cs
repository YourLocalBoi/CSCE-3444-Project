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
    public CameraCutscene postBattleCutscene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerPrefs.SetInt(enemyID, 1);   // MARK ENEMY AS DEAD
            PlayerPrefs.Save();
         

            trans.Play("FirstTransition");
            StartCoroutine(DelayFade());
            if (enemyID == "69")               
                postBattleCutscene.BeginCutscene();
            
        }
    }
    

    IEnumerator DelayFade()
    {
        yield return new WaitForSeconds(fadeTime);

        SceneManager.LoadScene(sceneToLoad);
    }
}
