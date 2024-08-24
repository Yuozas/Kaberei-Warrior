using MessagePack;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        string sceneToLoad = CurrentSave.Instance.SaveFile;
        sceneToLoad = MessagePackSerializer.Deserialize<SaveFile>(File.ReadAllBytes(Application.persistentDataPath + "/Saves/" + sceneToLoad)).CurrentScene;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
    }
}
