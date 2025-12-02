using System.Collections; 
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour 
{
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(WaitInUpdate());
            AudioManager.instance.Play(AudioManager.SoundType.Movement);
        }
        
    }

    IEnumerator WaitInUpdate()
    {
        yield return new WaitForSeconds(2f);
    }
}
