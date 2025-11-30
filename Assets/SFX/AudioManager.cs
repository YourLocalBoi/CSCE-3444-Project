using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoBehaviour 
{
    public enum SoundType //Enum to define different sound types
    {
        Movement, 
        Attack,
        BattleStart,
    }
    [System.Serializable]
    public class Sound
    {
        public SoundType type;
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 1f;

        [HideInInspector]
        public AudioSource source;
    }

    public static AudioManager instance;//Singleton instance
    public Sound[] allSounds; //Array to hold all sounds

    private Dictionary<SoundType, Sound> _soundDictionary = new Dictionary<SoundType, Sound>();
    private AudioSource _musicSource;
        //Dictionary to hold sounds for quick access

        private void Awake()
    {
        instance = this;

        foreach(var s in allSounds) //Populate dictionary with sounds
        {
            _soundDictionary[s.type] = s; //Add sound to dictionary
        }
    }
    public void Play(SoundType type)
    {
        if(!_soundDictionary.TryGetValue(type, out Sound s))
        {
            Debug.LogWarning($"Sound type {type} not found!"); //Warns if sound type is not found
            return; 
        }

        var soundObj = new GameObject($"Sound_{type}"); //Create new GameObject to play the sound
        var audioSource = soundObj.AddComponent<AudioSource>(); //Add AudioSource component to the new GameObject   

        audioSource.clip = s.clip; //Set clip
        audioSource.volume = s.volume; //Set volume

        audioSource.Play(); //Play the sound

        Destroy(soundObj, s.clip.length); //Destroys object after clip finishes playing

    }


}
