using UnityEngine;
using UnityEngine.UI;

public class SavePanel : MonoBehaviour
{
    public Text fileName;
    public Text timeStamp;
    public RectTransform rectTransform;
    public Button deleteSaveButton;
    public Button startGameButton;
    private void Awake()
    {
        fileName = transform.GetChild(0).gameObject.GetComponent<Text>();
        timeStamp = transform.GetChild(1).gameObject.GetComponent<Text>();
        deleteSaveButton = transform.GetChild(2).gameObject.GetComponent<Button>();
        startGameButton = transform.GetChild(3).gameObject.GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
    }
}
