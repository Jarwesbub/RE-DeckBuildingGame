using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartMenu : MonoBehaviour
{
    public CanvasGroup mainCanvas, reLogo;
    public TMP_Text pressAnyKey;
    private bool buttonLock;
    private bool isButtonPressed;

    private void OnEnable()
    {
        buttonLock = true;
        pressAnyKey.text = "";
        mainCanvas.alpha = 0;
        reLogo.alpha = 0;
    }

    private void Start()
    {
        string checkIfEmpty;
        string readFromFilePath;
        int count = 0;
        do //Checks all the available "MansionCards(number).text" -files and counts them
        {
            count++;
            checkIfEmpty = "MansionCards" + count;
            readFromFilePath = Application.persistentDataPath + "/Custom_data/" + checkIfEmpty + ".txt";
        }
        while (System.IO.File.Exists(readFromFilePath));

        count--;
        GameStats.MansionDeckCount = count;
        Debug.Log("MansionCards count = " + count);

        LeanTween.alphaCanvas(mainCanvas, 1f, 5f);
        StartCoroutine(ShowPressAnyKey());
    }
    public void FirstBootTextFileIsReady()
    {
        StartCoroutine(ButtonLockTime());
    }


    private void LogoStart()
    {
        LeanTween.alphaCanvas(reLogo, 1f, 4f);
    }

    private void Update()
    {
        if (Input.anyKey && !isButtonPressed && !buttonLock)
        {
            isButtonPressed = true;
            SceneManager.LoadScene("MainMenu");
        }
    }

    IEnumerator ShowPressAnyKey()
    {
        yield return new WaitForSeconds(4f);
        LogoStart();     
        yield return new WaitForSeconds(4f);
        pressAnyKey.text = "Press any key";
    }

    IEnumerator ButtonLockTime()
    {
        yield return new WaitForSeconds(2f);
        buttonLock = false;
    }
}
