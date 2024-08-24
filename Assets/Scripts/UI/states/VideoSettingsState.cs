public class VideoSettingsState : State<UIHandler>
{
    private static VideoSettingsState instance;
    private VideoSettingsState()
    {
        if (instance != null)
            return;
        instance = this;
    }
    public static VideoSettingsState Instance
    {
        get
        {
            if (instance == null)
                new VideoSettingsState();
            return instance;
        }
    }
    public override void EnterState(UIHandler owner)
    {
        owner.videoSettingsPanel.SetActive(true);
    }
    public override void ExitState(UIHandler owner)
    {
        owner.videoSettingsPanel.SetActive(false);
    }
    public override void UpdateState(UIHandler owner)
    {

    }
}
