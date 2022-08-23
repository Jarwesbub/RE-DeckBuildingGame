using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuControl : MonoBehaviour
{
    public void OnClickShowCustomdataPath()
    {
        string appdatapath = Application.persistentDataPath;

        //EditorUtility.RevealInFinder(appdatapath+ "/Custom_data/");
        System.Diagnostics.Process.Start(appdatapath + "/Custom_data/");
    }

    public void OnClickStartGame()
    {
        SceneManager.LoadScene("Loading");

    }
}
