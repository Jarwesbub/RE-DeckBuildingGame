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

        if (MainCanvas==null)
            MainCanvas = GameObject.FindWithTag("MainCanvas");

        string gameDataFilePath = Application.streamingAssetsPath + "/Game_data/";
        CreateFolder(gameDataFilePath);

        string customDataFilePath = Application.persistentDataPath + "/Custom_data/";
        CreateFolder(customDataFilePath);

        if (!System.IO.File.Exists(customDataFilePath + "MansionCards1.txt")) //Currently no need to create GameData -> When match starts it's then created
        {
            CreateTextFilesList();
            CreateCustomData();
            CreateMansionCards();
        }
        else
            Debug.Log("Data files exists!");

        if (!System.IO.File.Exists(customDataFilePath + "ShopCardsData1.txt"))
            CreateShopData();
        else
        {
            int i = 1;
            while (System.IO.File.Exists(customDataFilePath + "ShopCardsData" + i + ".txt"))
            {
                i++;
            }
            GameStats.ShopDeckDataCount = i-1; //Index values
        }

        if(!System.IO.File.Exists(customDataFilePath + "CharacterCards1.txt"))
        {
            CreateCharacterCards();
        }


        MainCanvas.GetComponent<StartMenu>().FirstBootTextFileIsReady(); //Disables button lock!
    }
    void CreateFolder(string folderName)
    {
        if (!Directory.Exists(folderName))
            Directory.CreateDirectory(folderName);
    }

    void CreateTextFilesList()
    {
        textFilesList = new string[3]; //Add+1
        textFilesList[0] = "CharacterList"; textFilesList[1] = "CharacterCustomlist"; textFilesList[2] = "StartingDeckList";
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
        foreach (string textFile in textFilesList)
        {
            string readFromStreamingAss = Application.streamingAssetsPath + "/Base_Data/" + textFile + ".txt";
            string[] fileLines = File.ReadAllLines(readFromStreamingAss).ToArray();

            string writeToFilePath = Application.persistentDataPath + "/Custom_data/" + textFile + ".txt";
            File.WriteAllLines(writeToFilePath, fileLines);
        }
    }

    void CreateMansionCards()
    {
        int cardsCount = 4;

        //MANSION CARDS
        string mansionReadTextFiles = Application.streamingAssetsPath + "/Base_Data/MansionCards1.txt";
        for (int i = 1; i <= cardsCount; i++) //Create "MansionCards1-4.txt"
        {
            if (i == 1)
            {
                string[] fileLines = File.ReadAllLines(mansionReadTextFiles).ToArray();
                string writeToCustomFolder = Application.persistentDataPath + "/Custom_data/MansionCards1.txt";
                File.WriteAllLines(writeToCustomFolder, fileLines);
            }
            else //Create empty text files with Mansion2-4 name
            {
                string[] fileLines = new string[1];
                fileLines[0] = "Mansion"+i;
                string writeToCustomFolder = Application.persistentDataPath + "/Custom_data/MansionCards" + i + ".txt";
                File.WriteAllLines(writeToCustomFolder, fileLines);
            }
        }
        GameStats.MansionDeckCount = cardsCount;

        Debug.Log("Custom_data - MansionCard files created!");
    }

    void CreateCharacterCards()
    {
        int cardsCount = 4;

        //CHARACTER CARDS
        CharacterCardsList allCharCards = new();
        List<string> characterCardsList = allCharCards.GetSupportedCharacterCardsByType(0).ToList();
        characterCardsList.Insert(0, "CharacterDeck1");

        for (int i = 1; i <= cardsCount; i++)
        {
            if (i == 1) //Base character cards listed in CharacterCardsList class
            {
                string writeToCustomFolder = Application.persistentDataPath + "/Custom_data/CharacterCards1.txt";
                File.WriteAllLines(writeToCustomFolder, characterCardsList);
            }
            else //Create empty text files with CharacterCards2-4 name
            {
                string[] fileLines = new string[1];
                fileLines[0] = "CharacterDeck" + i;
                string writeToCustomFolder = Application.persistentDataPath + "/Custom_data/CharacterCards" + i + ".txt";
                File.WriteAllLines(writeToCustomFolder, fileLines);
            }
        }

        GameStats.CharacterDeckCount = cardsCount;
        GameStats.CharacterDeckValue = 1;

    }

    void CreateShopData()
    {
        string streamingPath = Application.streamingAssetsPath;
        string readFromStreamingAss = streamingPath + "/Base_Data/ShopCardsData.txt";
        string[] fileLines = File.ReadAllLines(readFromStreamingAss).ToArray();

        string overwriteToFilePath = Application.streamingAssetsPath + "/Game_data/ShopCardsData.txt";
        File.WriteAllLines(overwriteToFilePath, fileLines);

        string persistentPath = Application.persistentDataPath;
        int cardsCount = 4; //TEST
        

        for (int i = 1; i <= cardsCount; i++) //Create "CustomShopCardsData1-4.txt"
        {
                string writeToCustomFolder = persistentPath + "/Custom_data/ShopCardsData" + i + ".txt";
            fileLines[0] = "Shop cards " + i;
                File.WriteAllLines(writeToCustomFolder, fileLines);

        }
        GameStats.ShopDeckDataCount = cardsCount;

        Debug.Log("Custom_data - ShopData files created!");
    }
}
