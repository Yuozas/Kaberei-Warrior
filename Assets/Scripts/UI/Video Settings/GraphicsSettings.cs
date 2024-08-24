using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
    Resolution[] resolutions;
    [SerializeField]
    Dropdown resolutionDropdown;
    [SerializeField]
    Toggle fullscreenToggle;
    [SerializeField]
    Dropdown qualityDropDown;
    [SerializeField]
    GameObject frameRateGameObject;
    [SerializeField]
    Toggle framerateToggle;

    void Start()
    {
        StartResolutions();
        StartOptions();
        LoadPrefs();
    }
    public void LoadPrefs()
    {
        LoadFullscreen(PlayerPrefs.GetInt("fullscreen") == 1);
        LoadResolutionAll(PlayerPrefs.GetInt("resolution-width"), PlayerPrefs.GetInt("resolution-height"), PlayerPrefs.GetInt("resolution"));
        LoadQuality(PlayerPrefs.GetInt("quality"));
        LoadFrameRate(PlayerPrefs.GetInt("framerate") == 1);

    }
    private void StartResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        foreach(Resolution resolution in resolutions)
        {
            options.Add(resolution.width + " x " + resolution.height);

        }
        resolutionDropdown.AddOptions(options);
    }
    private void StartOptions()
    {
        qualityDropDown.ClearOptions();
        List<string> options = new List<string>();

        foreach(string name in QualitySettings.names)
        {
            options.Add(name);
        }
        qualityDropDown.AddOptions(options);
    }
    private void LoadResolution(int resolutionIndex)
    {
        SetResolution(resolutionIndex);
        resolutionDropdown.value = resolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    private void LoadResolutionAll(int resolutionWidth, int resolutionHeight, int resolutionIndex)
    {
        bool resolutionExists = false;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutionWidth == resolutions[i].width)
            {
                if (resolutionHeight == resolutions[i].height)
                {
                    if (resolutionIndex == i)
                    {
                        resolutionExists = true;
                        break;
                    }
                }
            }
        }
        if (resolutionExists)
        {
            LoadResolution(resolutionIndex);
        }
        else
        {
            LoadResolution(0);
        }
    }
    private void LoadFullscreen(bool isFullScreen)
    {
        //Debug.Log("fullscreen loaded");
        SetFullScreen(isFullScreen);
        fullscreenToggle.isOn = isFullScreen;
    }
    private void LoadQuality(int qualityIndex)
    {
        SetQuality(qualityIndex);
        qualityDropDown.value = qualityIndex;
        qualityDropDown.RefreshShownValue();
    }
    private void LoadFrameRate(bool display)
    {
        DisplayFramerate(display);
        framerateToggle.isOn = display;
    }
    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolution-width", resolution.width);
        PlayerPrefs.SetInt("resolution-height", resolution.height);
        PlayerPrefs.SetInt("resolution", resolutionIndex);
    }
    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("quality", qualityIndex);
    }

    public void DisplayFramerate(bool display)
    {
        frameRateGameObject.SetActive(display);
        PlayerPrefs.SetInt("framerate", display ? 1 : 0);
    }
    public void Default()
    {
        DefaultRun.Construct(gameObject).DefaultGraphicsSettings();
        LoadPrefs();
    }
}