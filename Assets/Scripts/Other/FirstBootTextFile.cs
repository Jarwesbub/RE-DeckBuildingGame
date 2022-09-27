using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class FirstBootTextFile : MonoBehaviour
{
    private void Start()
    {
        CheckIfFoldersExist();
        CheckTextDataFolders();
    }
    void CheckIfFoldersExist()
    {
        //string overwrite_dataPath = Application.persistentDataPath + "/Game_data/";
        string overwrite_dataPath = Application.streamingAssetsPath + "/Game_data/";
        if (!Directory.Exists(overwrite_dataPath))
        {
            Directory.CreateDirectory(overwrite_dataPath);
        }

        string readCustom_dataPath = Application.persistentDataPath + "/Custom_data/";
        if (!Directory.Exists(readCustom_dataPath))
        {
            Directory.CreateDirectory(readCustom_dataPath);
        }
    }

    void CheckTextDataFolders() //
    {
        string checkIfEmpty = "CharacterList";
        //string readFromFilePath = Application.persistentDataPath + "/Game_data/" + checkIfEmpty + ".txt";
        string readFromFilePath = Application.streamingAssetsPath + "/Game_data/" + checkIfEmpty + ".txt";
        if (System.IO.File.Exists(readFromFilePath))
        {
            Debug.Log("Character list exists");
        }
        else
        {
            string[] shop_cards = new string[21]; //Add+1
            shop_cards[0] = "AmmoCountList"; shop_cards[1] = "ActionCardsList1_AllCounted"; shop_cards[2] = "ActionCardsList2_AllCounted";
            shop_cards[3] = "ActionCardsList3_AllCounted"; shop_cards[4] = "ActionCardsList4_AllCounted"; shop_cards[5] = "ActionCardsList5_AllCounted";
            shop_cards[6] = "ActionCardsList6_AllCounted"; shop_cards[7] = "ActionCardsList7_AllCounted"; shop_cards[8] = "CharacterList";
            shop_cards[9] = "CharacterCustomlist"; shop_cards[10] = "AR_SG_List_AllCounted"; shop_cards[11] = "GrenadeList_AllCounted";
            shop_cards[12] = "HandgunsList_AllCounted"; shop_cards[13] = "HPItemsList_AllCounted"; shop_cards[14] = "KnifeList_AllCounted";
            shop_cards[15] = "RifleList_AllCounted"; shop_cards[16] = "ShotgunList_AllCounted"; shop_cards[17] = "StartingDeckList";
            shop_cards[18] = "MainMansionCards"; shop_cards[19] = "Extra1List_AllCounted"; shop_cards[20] = "Extra2List_AllCounted";


            foreach (string textFile in shop_cards)
            {
                //string readFromStreamingAss = Application.streamingAssetsPath + "/Recall_Chat/" + textFile + ".txt";
                string readFromStreamingAss = Application.streamingAssetsPath + "/Base_Data/" + textFile + ".txt";
                string[] fileLines = File.ReadAllLines(readFromStreamingAss).ToArray();

                //string overwriteToFilePath = Application.persistentDataPath + "/Game_data/" + textFile + ".txt";
                string overwriteToFilePath = Application.streamingAssetsPath + "/Game_data/" + textFile + ".txt";
                File.WriteAllLines(overwriteToFilePath, fileLines);
            }

            //string readFromWrite_data = Application.streamingAssetsPath + "/Recall_Chat/" + checkIfEmpty + ".txt";
            string readFromWrite_data = Application.streamingAssetsPath + "/Base_Data/" + checkIfEmpty + ".txt";

            if (System.IO.File.Exists(readFromWrite_data))
            {
                foreach (string textFile in shop_cards)
                {
                    //string readFromStreamingAss = Application.streamingAssetsPath + "/Recall_Chat/" + textFile + ".txt";
                    string readFromStreamingAss = Application.streamingAssetsPath + "/Base_Data/" + textFile + ".txt";
                    string[] fileLines = File.ReadAllLines(readFromStreamingAss).ToArray();

                    string writeToFilePath = Application.persistentDataPath + "/Custom_data/" + textFile + ".txt";
                    File.WriteAllLines(writeToFilePath, fileLines);
                }

                for (int i = 1; i <= 4; i++) //Create "MansionCards1-4.txt"
                {
                    //string readFromStreamingAss = Application.streamingAssetsPath + "/Recall_Chat/MansionCards" + i + ".txt";
                    string readFromStreamingAss = Application.streamingAssetsPath + "/Base_Data/MansionCards" + i + ".txt";
                    string[] fileLines = File.ReadAllLines(readFromStreamingAss).ToArray();

                    //string writeToGameFolder = Application.persistentDataPath + "/Game_data/MansionCards" + i + ".txt";
                    string writeToGameFolder = Application.streamingAssetsPath + "/Game_data/MansionCards" + i + ".txt";
                    File.WriteAllLines(writeToGameFolder, fileLines);

                    string writeToCustomFolder = Application.persistentDataPath + "/Custom_data/MansionCards" + i + ".txt";
                    File.WriteAllLines(writeToCustomFolder, fileLines);
                }

                GameStats.MansionDeckCount = 4;

                Debug.Log("Write_data -text files created!");


            }

        }
        

    }


}
