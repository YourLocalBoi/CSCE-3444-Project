/*using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Audio;

public class optionMenu : MonoBehaviour
{

    public Toggle fullscreenTog, vsyncTog;

    public List<ResItem> resolutions = new List<ResItem>();
    private int selectedRes;

    public TMP_Text resolutionLabel;

    public AudioMixer theMixer;

    public TMP_Text mastLabel, musicLabel, sfxLabel;

    public Slider mastSlider, musicSlider, SFXSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;

        if(QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn=false;
        }
        else
        {
            vsyncTog.isOn = true;
        }

        bool foundRes = false;
        for(int i = 0; i < resolutions.Count; i++)
        {
            if(Screen.width ==  resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundRes = true;
                selectedRes = i;
                UpdateResLabel();
            }
            
        }

        if (!foundRes)
        {
            ResItem newRes= new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            resolutions.Add(newRes);
            selectedRes = resolutions.Count - 1;
            UpdateResLabel();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void resLeft()
    {
        selectedRes--;
        if(selectedRes < 0)
        {
            selectedRes = 0;
        }
        UpdateResLabel();
    }

    public void resRight()
    {
        selectedRes++;
        if(selectedRes > resolutions.Count - 1)
        {
            selectedRes = resolutions.Count - 1;
        }
        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedRes].horizontal.ToString() + " x " + resolutions[selectedRes].vertical.ToString();
    }
    public void applyGraphics(){
        //Screen.fullScreen = fullscreenTog.isOn;

        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
        Screen.SetResolution(resolutions[selectedRes].horizontal, resolutions[selectedRes].vertical, fullscreenTog.isOn);
    }

    public void SetMasterVolume()
    {
        mastLabel.text = Mathf.RoundToInt(mastSlider.value + 80).ToString();

        theMixer.SetFloat("Master Volume", mastSlider.value);
    }

     public void SetMuicVolume()
    {
        musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();

        theMixer.SetFloat("Music Volume", musicSlider.value);
    }
 public void SetSFXVolume()
    {
        sfxLabel.text = Mathf.RoundToInt(SFXSlider.value + 80).ToString();

        theMixer.SetFloat("SFX Volume", SFXSlider.value);
    }

}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}*/


using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Audio;

public class OptionMenu : MonoBehaviour
{
    [Header("Toggles")]
    public Toggle fullscreenTog;
    public Toggle vsyncTog;

    [Header("Resolution Settings")]
    public TMP_Text resolutionLabel;
    public List<ResItem> resolutions = new List<ResItem>();
    private int selectedRes = 0;

    [Header("Audio")]
    public AudioMixer theMixer;

    public TMP_Text mastLabel;
    public TMP_Text musicLabel;
    public TMP_Text sfxLabel;

    public Slider mastSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        //---------------------------
        // VISUAL SETTINGS
        //---------------------------
        fullscreenTog.isOn = Screen.fullScreen;
        vsyncTog.isOn = (QualitySettings.vSyncCount > 0);

        bool foundRes = false;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if (Screen.width == resolutions[i].horizontal &&
                Screen.height == resolutions[i].vertical)
            {
                selectedRes = i;
                foundRes = true;
                break;
            }
        }

        if (!foundRes)
        {
            ResItem newRes = new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;
            resolutions.Add(newRes);
            selectedRes = resolutions.Count - 1;
        }

        UpdateResLabel();

        //---------------------------
        // AUDIO SETTINGS
        //---------------------------

        float volumeValue;

        // Restore saved master volume
        if (theMixer.GetFloat("MasterVolume", out volumeValue))
        {
            mastSlider.value = volumeValue;
            mastLabel.text = Mathf.RoundToInt(volumeValue + 80).ToString();
        }

        // Restore saved music volume
        if (theMixer.GetFloat("MusicVolume", out volumeValue))
        {
            musicSlider.value = volumeValue;
            musicLabel.text = Mathf.RoundToInt(volumeValue + 80).ToString();
        }

        // Restore saved SFX volume
        if (theMixer.GetFloat("SFXVolume", out volumeValue))
        {
            sfxSlider.value = volumeValue;
            sfxLabel.text = Mathf.RoundToInt(volumeValue + 80).ToString();
        }

        // Make sure sliders are interactable
        mastSlider.interactable = true;
        musicSlider.interactable = true;
        sfxSlider.interactable = true;
    }


    // ---------------------------
    // RESOLUTION CONTROLS
    // ---------------------------

    public void ResLeft()
    {
        selectedRes--;
        if (selectedRes < 0)
            selectedRes = 0;

        UpdateResLabel();
    }

    public void ResRight()
    {
        selectedRes++;
        if (selectedRes > resolutions.Count - 1)
            selectedRes = resolutions.Count - 1;

        UpdateResLabel();
    }

    void UpdateResLabel()
    {
        resolutionLabel.text =
            resolutions[selectedRes].horizontal + " x " +
            resolutions[selectedRes].vertical;
    }

    public void ApplyGraphics()
    {
        QualitySettings.vSyncCount = vsyncTog.isOn ? 1 : 0;

        Screen.SetResolution(
            resolutions[selectedRes].horizontal,
            resolutions[selectedRes].vertical,
            fullscreenTog.isOn
        );
    }


    // ---------------------------
    // AUDIO VOLUME CONTROLS
    // ---------------------------
    
    public void SetMasterVolume()
    {
        theMixer.SetFloat("MasterVolume", mastSlider.value);
        mastLabel.text = Mathf.RoundToInt(mastSlider.value + 80).ToString();
    }

    public void SetMusicVolume()
    {
        theMixer.SetFloat("MusicVolume", musicSlider.value);
        musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
    }

    public void SetSFXVolume()
    {
        theMixer.SetFloat("SFXVolume", sfxSlider.value);
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal;
    public int vertical;
}
