using MessagePack;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstSceneDefeated : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text textUI;
    public void DefeatedScene(bool violence)
    {
        dialoguePanel.SetActive(true);
        if (violence)
        {
            StartCoroutine(LoadText("Now you've done it..."));
        }
        else
        {
            StartCoroutine(LoadText("You are interesting, let's have fun..."));
        }
    }
    public IEnumerator LoadText(string text)
    {
        foreach (char c in text)
        {
            textUI.text += c;
            yield return new WaitForSeconds(Input.anyKey? 0.1f: 0.01f);
        }
        while (!Input.anyKey)
        {
            yield return null;
        }
        Finish();
    }
    public void Finish()
    {
        SaveScene();
        SceneManager.LoadScene("1");
    }
    public void SaveScene()
    {
        SaveFile save = MessagePackSerializer.Deserialize<SaveFile>(File.ReadAllBytes(Application.persistentDataPath + "/Saves/" + CurrentSave.Instance.SaveFile));
        save.PlayerPositionX = 0;
        save.PlayerPositionY = 0;
        save.CurrentScene = "1";
        File.WriteAllBytes(Application.persistentDataPath + "/Saves/" + CurrentSave.Instance.SaveFile, MessagePackSerializer.Serialize(save));
    }
    
}
