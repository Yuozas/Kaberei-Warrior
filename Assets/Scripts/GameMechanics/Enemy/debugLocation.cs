using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugLocation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }
}
