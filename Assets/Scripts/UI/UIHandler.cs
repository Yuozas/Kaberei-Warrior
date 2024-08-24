using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    #region Main menu stuff
    public GameObject mainMenuPanel;
    #endregion
    #region Settings stuff
    public GameObject settingsPanel;
    #endregion
    #region Input settings stuff
    public GameObject inputSettingsPanel;
    #endregion
    #region Video settings stuff
    public GameObject videoSettingsPanel;
    #endregion
    #region Audio settings stuff
    public GameObject audioSettingsPanel;
    #endregion
    #region Save Files stuff
    public GameObject saveFilesPanel;
    #endregion
    public StateMachine<UIHandler> StateMachine { get; set; }
    private void Start()
    {
        StateMachine = new StateMachine<UIHandler>(this);
        DisablePanels();
        MainMenuStateStart();
    }
    private void Update()
    {
        StateMachine.Upate();
    }
    public void DisablePanels()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        inputSettingsPanel.SetActive(false);
        videoSettingsPanel.SetActive(false);
        audioSettingsPanel.SetActive(false);
        saveFilesPanel.SetActive(false);
    }

    #region States switching for buttons
    public void MainMenuStateStart()
    {
        StateMachine.ChangeState(MainMenuState.Instance);
    }
    public void SettingsStateStart()
    {
        StateMachine.ChangeState(SettingsState.Instance);
    }
    public void InputSettingsStateStart()
    {
        StateMachine.ChangeState(InputSettingsState.Instance);
    }
    public void VideoSettingsStateStart()
    {
        StateMachine.ChangeState(VideoSettingsState.Instance);
    }
    public void AudioSettingsStateStart()
    {
        StateMachine.ChangeState(AudioSettings.Instance);
    }
    public void SaveFilesStateStart()
    {
        StateMachine.ChangeState(SaveFilesState.Instance);
    }
    #endregion

}
