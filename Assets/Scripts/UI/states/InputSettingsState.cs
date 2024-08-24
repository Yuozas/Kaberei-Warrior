public class InputSettingsState : State<UIHandler>
{
    private static InputSettingsState instance;
    private InputSettingsState()
    {
        if (instance != null)
            return;
        instance = this;
    }
    public static InputSettingsState Instance
    {
        get
        {
            if (instance == null)
                new InputSettingsState();
            return instance;
        }
    }
    public override void EnterState(UIHandler owner)
    {
        owner.inputSettingsPanel.SetActive(true);
    }
    public override void ExitState(UIHandler owner)
    {
        owner.inputSettingsPanel.SetActive(false);
    }
    public override void UpdateState(UIHandler owner)
    {

    }
}
