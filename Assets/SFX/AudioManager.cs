using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public enum SoundType
    {
        Movement,
        Attack,
        Menu,
        Dialogue,
        MainMusic,
        BattleMusic
    }
    bool isMusic = false;

    [System.Serializable]
    public class Sound
    {
        public SoundType type;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        [HideInInspector]
        public AudioSource source; // Persistent AudioSource for looping
        public bool isMusic = false;
    }

    public static AudioManager instance;
    public Sound[] allSounds;

    private Dictionary<SoundType, Sound> _soundDictionary = new Dictionary<SoundType, Sound>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (var s in allSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = false; // default false, can be enabled when needed
            _soundDictionary[s.type] = s;
        }

        PlayMusic(SoundType.MainMusic);
        SceneManager.activeSceneChanged += OnSceneChanged;

    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        // Example logic – change these conditions to your needs!
        if (newScene.name.Contains("Battle"))
        {
            Crossfade(SoundType.BattleMusic);
        }
        else
        {
            Crossfade(SoundType.MainMusic);
        }
    }

    // ==========================================
    // Playback Functions
    // ==========================================

    public void Play(SoundType type)
    {
        if (_soundDictionary.TryGetValue(type, out Sound s))
        {
            s.source.PlayOneShot(s.clip, s.volume);
        }
    }

    public void PlayLoop(SoundType type)
    {
        if (_soundDictionary.TryGetValue(type, out Sound s))
        {
            if (!s.source.isPlaying)
            {
                s.source.loop = true;
                s.source.Play();
            }
        }
    }


    public void PlayMusic(SoundType type)
    {
        if (_soundDictionary.TryGetValue(type, out Sound s))
        {
            // Stop all other looping music
            StopAllMusic();

            s.source.loop = true;
            s.source.Play();
        }
    }

    public void Stop(SoundType type)
    {
        if (_soundDictionary.TryGetValue(type, out Sound s))
        {
            s.source.Stop();
            s.source.loop = false;
        }
    }
   

    private void StopAllMusic()
    {
        foreach (var s in allSounds)
        {
            if (s.isMusic)
            {
                if (s.source.loop)
                {
                    s.source.Stop();
                    s.source.loop = false;
                }
            }
        }
    }

    // Smooth transition between tracks
    private void Crossfade(SoundType newMusic)
    {
        {
            StopAllMusic();
            PlayMusic(newMusic);
        }
    }
}