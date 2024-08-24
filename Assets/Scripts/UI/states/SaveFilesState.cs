using MessagePack;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveFilesState : State<UIHandler>
{
    private static SaveFilesState instance;
    private SaveFilesState()
    {
        if (instance != null)
            return;
        instance = this;
    }
    public static SaveFilesState Instance
    {
        get
        {
            if (instance == null)
                new SaveFilesState();
            return instance;
        }
    }
    public override void EnterState(UIHandler owner)
    {
        owner.saveFilesPanel.SetActive(true);

        DirectoryInfo saveDirectory = new DirectoryInfo(Application.persistentDataPath + "/Saves/");
        FileInfo[] saveFiles = saveDirectory.GetFiles("*.kabereiSave");
        for(int i=0; i<saveFiles.Length;i++)
        {
            SaveFile saveFile = MessagePackSerializer.Deserialize<SaveFile>(File.ReadAllBytes(saveFiles[i].FullName));
            SaveFilesPanel saveFilesPanel = owner.saveFilesPanel.GetComponent<SaveFilesPanel>();
            saveFilesPanel.AddSave(saveFiles[i].Name, saveFile.Timestamp.ToString(), i+1);
        }
    }
    public override void ExitState(UIHandler owner)
    {
        owner.saveFilesPanel.GetComponent<SaveFilesPanel>().DestroyLoadedSaves();
        owner.saveFilesPanel.SetActive(false);
    }
    public override void UpdateState(UIHandler owner)
    {

    }

}