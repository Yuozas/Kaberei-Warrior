using MessagePack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NewGame : MonoBehaviour
{
    public void StartNewGame()
    {
        StartCoroutine(NewGameCoroutine());//To make a file with new name but not overright existing
    }
    private IEnumerator NewGameCoroutine()
    {
        DirectoryInfo saveDirectory = new DirectoryInfo(Application.persistentDataPath + "/Saves/");
        FileInfo[] fileInfos = saveDirectory.GetFiles("*.kabereiSave");
        float id = fileInfos.Length;
        bool loop = true;
        if (id != 0)
        {
            while (loop)
            {
                foreach (FileInfo fileInfo in fileInfos)
                {
                    //Debug.Log("\nfileInfo.Name:   " + fileInfo.Name + "\nid.kabereiSave:   " + id + ".kabereiSave");
                    if (fileInfo.Name == id + ".kabereiSave")
                    {
                        id++;
                        loop = true;
                        break;
                    }
                    else
                    {
                        loop = false;
                    }
                }
            }
        }

        SaveFile saveData = new SaveFile
        {
            Timestamp = DateTime.UtcNow,
            PlayerPositionX = -60,
            PlayerPositionY = 0,
            CurrentScene = "0"
        };
        File.WriteAllBytes(Application.persistentDataPath + "/Saves/" + id + ".kabereiSave", MessagePackSerializer.Serialize(saveData));
        CurrentSave.Instance.SaveFile = id + ".kabereiSave";
        
        //UnityEditor.AssetDatabase.Refresh();
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameLoader");
        yield break;
    }
}
