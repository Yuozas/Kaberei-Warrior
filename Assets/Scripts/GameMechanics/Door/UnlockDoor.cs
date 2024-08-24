using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    [SerializeField]
    GameObject door;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        door.SetActive(false);
        gameObject.SetActive(false);
    }
}
