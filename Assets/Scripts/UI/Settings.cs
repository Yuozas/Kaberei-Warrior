using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    GraphicsSettings graphicsSettings;
    MasterVolume masterVolume;
    Bind bind;
    public void SetIfNull()
    {
        if(graphicsSettings == null)
            graphicsSettings = GetComponent<GraphicsSettings>();
        if(masterVolume == null)
            masterVolume = GetComponent<MasterVolume>();
        if(bind == null)
            bind = GetComponent<Bind>();
    }
    public void Default()
    {
        DefaultRun.Construct(gameObject).DefaultAllSettings();
        SetIfNull();
        graphicsSettings.LoadPrefs();
        masterVolume.LoadPrefs();
        bind.LoadPrefs();

    }
}
