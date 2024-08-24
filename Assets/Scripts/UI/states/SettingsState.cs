public class SettingsState : State<UIHandler>
{
    private static SettingsState instance;
    private SettingsState()
    {
        if (instance != null)
            return;
        instance = this;
    }
    public static SettingsState Instance
    {
        get
        {
            if (instance == null)
                new SettingsState();
            return instance;
        }
    }
    public override void EnterState(UIHandler owner)
    {
        owner.settingsPanel.SetActive(true);
    }
    public override void ExitState(UIHandler owner)
    {
        owner.settingsPanel.SetActive(false);
    }
    public override void UpdateState(UIHandler owner)
    {

    }
}
