public class AudioSettings : State<UIHandler>
{
    private static AudioSettings instance;
    private AudioSettings()
    {
        if (instance != null)
            return;
        instance = this;
    }
    public static AudioSettings Instance
    {
        get
        {
            if (instance == null)
                new AudioSettings();
            return instance;
        }
    }
    public override void EnterState(UIHandler owner)
    {
        owner.audioSettingsPanel.SetActive(true);
    }
    public override void ExitState(UIHandler owner)
    {
        owner.audioSettingsPanel.SetActive(false);
    }
    public override void UpdateState(UIHandler owner)
    {

    }

}

