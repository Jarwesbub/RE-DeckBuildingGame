using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShowcaseZoomCard : MonoBehaviour
{
    private bool isZoomed;
    [SerializeField] private Vector2 normalSize, zoomedSize;

    public void OnClickZoomCard()
    {
        if(isZoomed)
        {
            transform.localScale = normalSize;
            isZoomed = false;
        }
        else
        {
            transform.localScale = zoomedSize;
            isZoomed = true;
        }


    }
}
