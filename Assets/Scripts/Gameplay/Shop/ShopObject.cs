using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopObject : MonoBehaviour
{
    public GameObject ShopScrollBar;
    private bool notGameStart;

    private void OnEnable()
    {
        if (notGameStart) //Skip first "OnEnable" -event -> ShopCards can't keep up with PhotonView
            ShopScrollBar.GetComponent<Scrollbar>().value = 1;
        else
            notGameStart = true;
    }


}
