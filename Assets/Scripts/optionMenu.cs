using UnityEngine;
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
}