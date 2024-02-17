using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Michsky.UI.Shift;

public class SettingManager : MonoBehaviour
{
    public SwitchManager vsyncButton, fullscreenButton, fpsButton;
    public List<ResItem> resolutions = new List<ResItem>();
    private int selectedResolution;

    public TMP_Text resolutionLabel, fpsText;

    private Resolution[] res;
    private List<Resolution> filteredResolutions;
    public TMP_Dropdown resolutionDropdown;
    private float currentRefreshRate;
    private int currentResolutionIndex = 0;

    // FPS Variables
    private int frameIndex;
    private float[] frameDeltaTimeArray;

    // Start is called before the first frame update
    void Start()
    {
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

    }

    private void GrabScreenResolution()
    {
        frameDeltaTimeArray = new float[50];

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

    // Update is called once per frame
    void Update()
    {
        DisplayFPS();
    }

    /*
    public void ResLeft()
    {
        selectedResolution--;
        if(selectedResolution < 0)
        {
            selectedResolution = 0;
        }

        UpdateResLabel();
    }

    public void ResRight()
    {
        selectedResolution++;
        if(selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = resolutions.Count - 1; 
        }
        UpdateResLabel();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " X " + resolutions[selectedResolution].vertical.ToString();
    }*/

    public void ApplySettings()
    {
        Screen.fullScreen = fullscreenButton.isOn;

        if (vsyncButton.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenButton.isOn);
    }

    private void DisplayFPS()
    {
        frameDeltaTimeArray[frameIndex] = Time.deltaTime;
        frameIndex = (frameIndex + 1) % frameDeltaTimeArray.Length;

        fpsText.text = "FPS : " + Mathf.RoundToInt(CalculateFPS()).ToString();
    }

    private float CalculateFPS()
    {
        float total = 0f;
        foreach (float deltaTime in frameDeltaTimeArray)
        {
            total += deltaTime;
        }

        return frameDeltaTimeArray.Length / total;
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
