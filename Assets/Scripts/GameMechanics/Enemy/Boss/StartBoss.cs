using MessagePack;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StartBoss : MonoBehaviour
{
    public PlayerMechanics player;
    public GameObject door;
    public GameObject boss;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            door.SetActive(true);
            boss.SetActive(true);
            player.SaveGame();
            gameObject.SetActive(false);
        }
    }

}
