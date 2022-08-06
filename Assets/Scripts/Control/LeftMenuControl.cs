using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LeftMenuControl : MonoBehaviourPun
{
    public GameObject LeftMenu, OtherSettingsMenu;
    public Text plus1;
    private bool isVisible, isSettingsVisible;
    

    private void Start()
    {
        LeftMenu.SetActive(false);
        OtherSettingsMenu.SetActive(false);
        isVisible = false;
        isSettingsVisible = false;
        plus1.text = "";
    }

    public void SetPlus1Text()
    {
        plus1.text = "+1";
    }

    public void OnClickLeftMenuButton()
    {
        plus1.text = "";
        if (isVisible)
        {
            LeftMenu.SetActive(false);
            isVisible = false;
        }
        else
        {
            LeftMenu.SetActive(true);
            isVisible = true;
        }
    }
    public void OnClickOtherSettingsButton()
    {
        if (isSettingsVisible)
        {
            OtherSettingsMenu.SetActive(false);
            isSettingsVisible = false;
        }
        else
        {
            OtherSettingsMenu.SetActive(true);
            isSettingsVisible = true;
        }

    }
}
