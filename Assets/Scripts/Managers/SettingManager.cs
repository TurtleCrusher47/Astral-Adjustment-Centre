using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Michsky.UI.Shift;
using UnityEngine.Audio;
using Unity.VisualScripting;
using UnityEngine.PlayerLoop;

public class SettingManager : MonoBehaviour
{
    public SwitchManager vsyncButton, fullscreenButton, fpsButton;

    public GameObject fpsBox;

    private Resolution[] res;
    private List<Resolution> filteredResolutions;
    public TMP_Dropdown resolutionDropdown;
    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    [SerializeField] private AudioMixer globalMixer;
    [SerializeField] private Slider bgmSlider, sfxSlider, vlSlider;

    private float bgmBefore, sfxBefore, vlBefore;

    public bool appliedSettings = false, localFullscreenBool;

    void Awake()
    {
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume");
        }
        else
        {
            bgmSlider.value = 1;
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        }
        else
        {
            sfxSlider.value = 1;
        }

        if (PlayerPrefs.HasKey("VLVolume"))
        {
            vlSlider.value = PlayerPrefs.GetFloat("VLVolume");
        }
        else
        {
            vlSlider.value = 1;
        }

        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            string localFullscreen = PlayerPrefs.GetString("Fullscreen");

            if (localFullscreen.ToLower() == "true")
            {
                localFullscreenBool = true;
            }
            else if (localFullscreen.ToLower() == "false")
            {
                localFullscreenBool = false;
            }

            Screen.fullScreen = localFullscreenBool;

            Debug.Log("Loaded PlayerPrefs (FullscreenBool) : " + localFullscreenBool);
        }
        else
        {
            Screen.fullScreen = true;
        }
    }

    void Start()
    {
        globalMixer.SetFloat("BGMVolume", Mathf.Log10(bgmSlider.value) * 20);
        globalMixer.SetFloat("SFXVolume", Mathf.Log10(sfxSlider.value) * 20);
        globalMixer.SetFloat("VLVolume", Mathf.Log10(vlSlider.value) * 20);

        GrabScreenResolution();

        fullscreenButton.isOn = Screen.fullScreen;

        if(QualitySettings.vSyncCount == 0)
        {
            vsyncButton.isOn = false;
        }
        else
        {
            vsyncButton.isOn = true;
        }

        // Set the initial state of fpsButton to false
        fpsButton.isOn = false;
    }

    public void ResetAppliedSettingsBool()
    {
        appliedSettings = false;
    }

    public void SetInitialVolumes()
    {
        bgmBefore = bgmSlider.value;
        sfxBefore = sfxSlider.value;
        vlBefore = vlSlider.value;
    }

    public void ResetVolumes()
    {
        if (!appliedSettings)
        {
            bgmSlider.value = bgmBefore;
            sfxSlider.value = sfxBefore;
            vlSlider.value = vlBefore;

            globalMixer.SetFloat("BGMVolume", Mathf.Log10(bgmSlider.value) * 20);
            globalMixer.SetFloat("SFXVolume", Mathf.Log10(sfxSlider.value) * 20);
            globalMixer.SetFloat("VLVolume", Mathf.Log10(vlSlider.value) * 20);
        }
    }

    public void SetBGMVolume(float volume)
    {
        globalMixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        globalMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    public void SetVLVolume(float volume)
    {
        globalMixer.SetFloat("VLVolume", Mathf.Log10(volume) * 20);
    }

    private void GrabScreenResolution()
    {
        res = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < res.Length; i++)
        {
            if (res[i].refreshRate == currentRefreshRate)
            {
                filteredResolutions.Add(res[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRate + " Hz";
            options.Add(resolutionOption);

            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resIndex)
    {
        Resolution resolution = filteredResolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, true);
    }

    public void ApplySettings()
    {
        PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.SetFloat("VLVolume", vlSlider.value);

        Screen.fullScreen = fullscreenButton.isOn;

        if (vsyncButton.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Debug.Log("Vsync Active " + vsyncButton.isOn);
        Debug.Log("FullScreen Active " + Screen.fullScreen);
    
        // Save the state of the fpsButton to PlayerPrefs
        PlayerPrefs.SetInt("FPSButtonState", fpsButton.isOn ? 1 : 0);

        appliedSettings = true;

        PlayerPrefs.Save();

        // Show or hide FPS based on the state of the fpsButton
        if (fpsButton.isOn)
        {
            ShowFPS();
        }
        else
        {
            HideFPS();
        }
    }

    private void HideFPS()
    {
        fpsBox.SetActive(false);
    }

    private void ShowFPS()
    {
        fpsBox.SetActive(true);
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
