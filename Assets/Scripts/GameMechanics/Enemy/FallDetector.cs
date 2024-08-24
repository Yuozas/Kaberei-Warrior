using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetector : MonoBehaviour
{
    Transform detector;
    RaycastHit2D hit;
    private void Awake()
    {
        detector = transform;
    }

    public bool NoFloor()
    {
        
        hit = Physics2D.Raycast(detector.position, Vector2.down, 1.5f);
        /*
        Color color;
        if (hit)
            color = Color.red;
        else
            color = Color.green;
        Debug.DrawRay(detector.position, Vector2.down * 1.5f, color);
        */
        return !hit;
    }
}
