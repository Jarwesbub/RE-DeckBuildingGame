using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuControl : MonoBehaviour
{
    public InputField AppDataPath;

    private void Start()
    {
        //string appdatapath = Application.dataPath;
        string appdatapath = Application.persistentDataPath;
        AppDataPath.text = appdatapath + "/Custom_data/";
    }

    public void OnClickStartGame()
    {
        SceneManager.LoadScene("Loading");

    }
}
