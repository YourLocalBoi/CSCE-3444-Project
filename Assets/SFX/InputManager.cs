using System.Collections; 
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour 
{
    public CollisionDialogue dialogueT;
    private bool dialogueSoundPlayed = false;
    void Update()
    {
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                        Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (isMoving)
        {
            AudioManager.instance.PlayLoop(AudioManager.SoundType.Movement);
        }
        else
        {
            AudioManager.instance.Stop(AudioManager.SoundType.Movement);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManager.instance.Play(AudioManager.SoundType.Menu);
            
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.instance.Play(AudioManager.SoundType.SkipDialogue);
            
        }
        if (dialogueT.hasTriggered && !dialogueSoundPlayed)
        {
            AudioManager.instance.Play(AudioManager.SoundType.Dialogue);
            dialogueSoundPlayed = true;
            
        }
    }


}
