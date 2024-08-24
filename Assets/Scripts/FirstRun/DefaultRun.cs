using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefaultRun : MonoBehaviour
{
    bool menu = false;
    public static DefaultRun Construct(GameObject gameObject)
    {
        DefaultRun defaultRun = gameObject.AddComponent<DefaultRun>();
        defaultRun.menu = true;
        return defaultRun;
    }
    void Start()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves/"))
        {
            //if it doesn't, create it
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves/");

        }
        StartCoroutine(StartWithAnim());
    }
    IEnumerator StartWithAnim()
    {
        if (menu) yield break;

        if (PlayerPrefs.GetInt("SetDefaull") == 0)
        {
            //Set default settings
            DefaultAllSettings();
        }
        LoadMainMenu();
    }
    public void DefaultAllSettings()
    {

        //Graphics
        DefaultGraphicsSettings();

        //Audio
        DefaultAudioSettings();

        //Input
        DefaultInputSettings();

        PlayerPrefs.SetInt("SetDefaull", 1);
    }
    public void DefaultGraphicsSettings()
    {
        PlayerPrefs.SetInt("fullscreen", 1);//Fullscreen true
        SetDefaultResolution();
        PlayerPrefs.SetInt("quality", QualitySettings.names.Length-1);//Ultra Quality
        PlayerPrefs.SetInt("framerate", 0);
    }
    public void DefaultAudioSettings()
    {
        PlayerPrefs.SetFloat("masterVolumeValue", 0);// +0 DB
    }
    public void DefaultInputSettings()
    {
        PlayerPrefs.SetString("attackInput", KeyCode.E.ToString());
        PlayerPrefs.SetString("jumpInput", KeyCode.Space.ToString());
        PlayerPrefs.SetString("walkLeftInput", KeyCode.A.ToString());
        PlayerPrefs.SetString("walkRightInput", KeyCode.D.ToString());
        PlayerPrefs.SetString("runInput", KeyCode.LeftShift.ToString());
        PlayerPrefs.SetString("attackModeInput", KeyCode.Q.ToString());
    }
    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void SetDefaultResolution()
    {
        Resolution[] resolutions = Screen.resolutions;
        int index = resolutions.Length - 1;
        PlayerPrefs.SetInt("resolution", index);
        PlayerPrefs.SetInt("resolution-width", resolutions[index].width);
        PlayerPrefs.SetInt("resolution-height", resolutions[index].height);
    }
}
