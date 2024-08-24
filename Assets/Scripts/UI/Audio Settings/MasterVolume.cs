using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MasterVolume : MonoBehaviour
{
    [SerializeField]
    AudioMixer audioMixer;
    [SerializeField]
    Slider slider;

    void Start()
    {
        LoadPrefs();
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("masterVolumeValue", volume);
    }
    public void LoadPrefs()
    {
        slider.value = PlayerPrefs.GetFloat("masterVolumeValue");
    }
    public void Default()
    {
        DefaultRun.Construct(gameObject).DefaultAudioSettings();
        LoadPrefs();
    }
}
