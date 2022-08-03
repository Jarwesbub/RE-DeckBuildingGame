using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardholder : MonoBehaviour
{
    private bool mousePressed;

    void OnMouseDown()
    {
        mousePressed = true;

    }
    void OnMouseUp()
    {
        mousePressed = false;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if(mousePressed)
        if(other.gameObject.tag=="cardPrefab")
            {


            }


    }



}
