using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitGameplay : MonoBehaviour
{
    [SerializeField]
    GameObject ExitPanel;
    [SerializeField]
    Button continueGamePlay, exitToMainMenu;
    [SerializeField]
    PlayerMechanics playerMechanics;

    // Start is called before the first frame update
    void Start()
    {
        continueGamePlay.onClick.AddListener(() =>
        {
            endPanel();
        });
        exitToMainMenu.onClick.AddListener(() =>
        {
            playerMechanics.SaveGame();
            endPanel();
            SceneManager.LoadScene("MainMenu");
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ExitPanel.activeInHierarchy)
            {
                endPanel();
            }
            else
            {
                startPanel();
            }
        }
    }
    private void startPanel()
    {
        ExitPanel.SetActive(true);
        Time.timeScale = 0;
    }
    private void endPanel()
    {
        ExitPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
