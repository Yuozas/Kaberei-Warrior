using UnityEngine;
using UnityEngine.UI;

public class FrameRate : MonoBehaviour
{
    Text text;
    private float deltaTime;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        text.text = Mathf.Ceil(1.0f / deltaTime).ToString();
    }
}
