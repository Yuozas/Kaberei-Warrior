using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveFilesPanel : MonoBehaviour
{
    [SerializeField]
    GameObject savePanelPrefab;
    private List<GameObject> saveGameObjects = new List<GameObject>();
    public void AddSave(string filename, string timestamp, int order)
    {
        GameObject savePanelGameObject = Instantiate(savePanelPrefab);
        savePanelGameObject.transform.SetParent(gameObject.transform,false);
        SavePanel savePanel = savePanelGameObject.GetComponent<SavePanel>();
        savePanel.fileName.text = filename;
        savePanel.timeStamp.text = timestamp;
        savePanel.deleteSaveButton.onClick.AddListener(() => {
            File.Delete(Application.persistentDataPath + "/Saves/" + filename);
            //UnityEditor.AssetDatabase.Refresh();
            Destroy(savePanelGameObject);
        });
        savePanel.startGameButton.onClick.AddListener(() => {
            CurrentSave.Instance.SaveFile = filename;
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameLoader");
        });
        savePanel.rectTransform.position = new Vector3(savePanel.rectTransform.position.x, savePanel.rectTransform.position.y - 30 * order, savePanel.rectTransform.position.z);
        saveGameObjects.Add(savePanelGameObject);
    }
    public void DestroyLoadedSaves()
    {
        foreach (GameObject gameObject in saveGameObjects)
        {
            Destroy(gameObject);
        }
        saveGameObjects = new List<GameObject>();
    }
}
