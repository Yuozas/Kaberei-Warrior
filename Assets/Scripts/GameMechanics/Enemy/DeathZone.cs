using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    bool died = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!died)
            collision.gameObject.SendMessage("ObjetcDie");
    }
}
