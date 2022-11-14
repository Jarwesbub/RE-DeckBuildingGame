using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject StartMenu, LoadingScreen, OptionsMenu, CustomizeMenu; //MainMenu loading screen

    private void Start()
    {
        StartMenu.SetActive(true);
        LoadingScreen.SetActive(false);
        CustomizeMenu.SetActive(false);
        OptionsMenu.SetActive(false);
    }


    public void OnClickShowCustomdataPath()
    {
        string appdatapath = Application.persistentDataPath;

        //EditorUtility.RevealInFinder(appdatapath+ "/Custom_data/");
        System.Diagnostics.Process.Start(appdatapath + "/Custom_data/");
    }

    public void OnClickStartGame() //MAIN MENU
    {
        SceneManager.LoadScene("Loading");//Doesn't need main menu loading screen -> connects to server instead

    }
    public void OnClickQuitGame()
    {
        Application.Quit();
    }

    public void OnClickOptions(bool come)//OPTIONS MENU
    {
        StartMenu.SetActive(!come);
        OptionsMenu.SetActive(come);

    }
    public void OnClickCustomize(bool come)//OPTIONS MENU
    {
        StartMenu.SetActive(!come);
        CustomizeMenu.SetActive(come);

    }
    public void OnClickMansionBuilder() //OPTIONS MENU
    {
        LoadingScreen.SetActive(true);
        SceneManager.LoadScene("EditMansion");

    }
    public void OnClickShopBuilder() //OPTIONS MENU
    {
        LoadingScreen.SetActive(true);
        SceneManager.LoadScene("EditShop");

    }
}
