public class MainMenuState : State<UIHandler>
{
    private static MainMenuState instance;
    private MainMenuState()
    {
        if (instance != null)
            return;
        instance = this;
    }
    public static MainMenuState Instance
    {
        get
        {
            if (instance == null)
                new MainMenuState();
            return instance;
        }
    }
    public override void EnterState(UIHandler owner)
    {
        owner.mainMenuPanel.SetActive(true);
    }
    public override void ExitState(UIHandler owner)
    {
        owner.mainMenuPanel.SetActive(false);
    }
    public override void UpdateState(UIHandler owner)
    {

    }
}
