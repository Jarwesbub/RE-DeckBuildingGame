using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuControl : MonoBehaviour
{
    private void Start()
    {
        string checkIfEmpty;
        string readFromFilePath;
        int count = 0;
            do
            {
                count++;
                checkIfEmpty = "MansionCards"+count;
                readFromFilePath = Application.persistentDataPath + "/Custom_data/" + checkIfEmpty + ".txt";
            }
            while (System.IO.File.Exists(readFromFilePath));

        count--;

        GameStats.MansionDeckCount = count;

        Debug.Log("MansionCards count = " + count);

        
    }


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

    public void OnClickMansionBuilder()
    {
        SceneManager.LoadScene("EditMansion");

    }
}
