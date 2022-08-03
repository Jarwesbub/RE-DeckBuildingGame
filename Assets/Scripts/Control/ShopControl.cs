using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopControl : MonoBehaviour
{
    public GameObject MainCanvasControl;
    public GameObject Shop_HandgunsListPrefab;

    private void Awake()
    {
        if (MainCanvasControl == null)
            MainCanvasControl = GameObject.FindWithTag("MainCanvas");
        
    }


}
