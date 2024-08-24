using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlide : MonoBehaviour
{
    PlayerMechanics playerMechanics;
    public bool blockedMovement;
    private void Awake()
    {
        playerMechanics = GetComponent<PlayerMechanics>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
            blockedMovement = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
            blockedMovement = false;
    }
}
