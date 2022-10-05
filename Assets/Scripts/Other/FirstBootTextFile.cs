using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class FirstBootTextFile : MonoBehaviour
{
    private GameObject MainCanvas;
    private string[] textFilesList;

    private void Start()
    {
        if(MainCanvas==null)
            MainCanvas = GameObject.FindWithTag("MainCanvas");

        string gameDataFilePath = Application.streamingAssetsPath + "/Game_data/";
        CreateFolder(gameDataFilePath);

        string customDataFilePath = Application.persistentDataPath + "/Custom_data/";
        CreateFolder(customDataFilePath);

        gameDataFilePath += "CharacterList.txt";
        customDataFilePath += "MansionCards1.txt";

        bool createNewGameData = !System.IO.File.Exists(gameDataFilePath);
        bool createNewCustomData = !System.IO.File.Exists(customDataFilePath);

        if (createNewGameData || createNewCustomData)
        {
            CreateTextFilesList();

            if (createNewCustomData) CreateGameData();

            if (createNewCustomData) CreateCustomData();
        }
        else
            Debug.Log("Data files exists!");

        MainCanvas.GetComponent<StartMenu>().FirstBootTextFileIsReady(); //Disables button lock!
    }
    void CreateFolder(string folderName)
    {
        if (!Directory.Exists(folderName))
            Directory.CreateDirectory(folderName);
    }

    void CreateTextFilesList()
    {
        textFilesList = new string[21]; //Add+1
        textFilesList[0] = "AmmoCountList"; textFilesList[1] = "ActionCardsList1_AllCounted"; textFilesList[2] = "ActionCardsList2_AllCounted";
        textFilesList[3] = "ActionCardsList3_AllCounted"; textFilesList[4] = "ActionCardsList4_AllCounted"; textFilesList[5] = "ActionCardsList5_AllCounted";
        textFilesList[6] = "ActionCardsList6_AllCounted"; textFilesList[7] = "ActionCardsList7_AllCounted"; textFilesList[8] = "CharacterList";
        textFilesList[9] = "CharacterCustomlist"; textFilesList[10] = "AR_SG_List_AllCounted"; textFilesList[11] = "GrenadeList_AllCounted";
        textFilesList[12] = "HandgunsList_AllCounted"; textFilesList[13] = "HPItemsList_AllCounted"; textFilesList[14] = "KnifeList_AllCounted";
        textFilesList[15] = "RifleList_AllCounted"; textFilesList[16] = "ShotgunList_AllCounted"; textFilesList[17] = "StartingDeckList";
        textFilesList[18] = "MainMansionCards"; textFilesList[19] = "Extra1List_AllCounted"; textFilesList[20] = "Extra2List_AllCounted";
    }

    void CreateGameData()
    {
        foreach (string textFile in textFilesList)
        {
            string readFromStreamingAss = Application.streamingAssetsPath + "/Base_Data/" + textFile + ".txt";
            string[] fileLines = File.ReadAllLines(readFromStreamingAss).ToArray();

            string overwriteToFilePath = Application.streamingAssetsPath + "/Game_data/" + textFile + ".txt";
            File.WriteAllLines(overwriteToFilePath, fileLines);
        }
        Debug.Log("Game_data files created!");
    }


    void CreateCustomData() //
    {
        int mansionCardsCount = 4;

        foreach (string textFile in textFilesList)
        {
            string readFromStreamingAss = Application.streamingAssetsPath + "/Base_Data/" + textFile + ".txt";
            string[] fileLines = File.ReadAllLines(readFromStreamingAss).ToArray();

            string writeToFilePath = Application.persistentDataPath + "/Custom_data/" + textFile + ".txt";
            File.WriteAllLines(writeToFilePath, fileLines);
        }

        for (int i = 1; i <= mansionCardsCount; i++) //Create "MansionCards1-4.txt"
        {
            string readFromStreamingAss = Application.streamingAssetsPath + "/Base_Data/MansionCards" + i + ".txt";
            string[] fileLines = File.ReadAllLines(readFromStreamingAss).ToArray();

            string writeToGameFolder = Application.streamingAssetsPath + "/Game_data/MansionCards" + i + ".txt";
            File.WriteAllLines(writeToGameFolder, fileLines);

            string writeToCustomFolder = Application.persistentDataPath + "/Custom_data/MansionCards" + i + ".txt";
            File.WriteAllLines(writeToCustomFolder, fileLines);
        }
        GameStats.MansionDeckCount = mansionCardsCount;

        Debug.Log("Custom_data files created!");

    }


}
