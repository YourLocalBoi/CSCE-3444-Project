using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class SceneChangeForBattle : MonoBehaviour
{
    public string sceneToLoad;//scene to load on collision
    public Animator trans;//animator for transition
    public float fadeTime = 1f;//fade time duration
    public bool sceneHasChanged = false;//flag to check if scene has changed
    public BattleHandler bH;
    private void changeScene()//change scene on collision function
    {   
        if(bH.changeSceneFlag == true)
            trans.Play("FirstTransition");//play transition animation)
            StartCoroutine(DelayFade());//start delay fade coroutine
            sceneHasChanged = true;
        
    }

    IEnumerator DelayFade()
    {
        yield return new WaitForSeconds(fadeTime);//wait for 1 second
        SceneManager.LoadScene(sceneToLoad);

    }

}