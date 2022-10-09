using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Photon.Pun;

//Attach this script to object that contains "LobbyRoomOpen.cs" -script

public class OverwriteTextFileList : MonoBehaviourPun
{
    [SerializeField] GameObject AllShopCards;
    private bool isMaster,buttonIsPressed;
    //[SerializeField] private string[] decklist;
    private string[] fileLines, shopDataFileNames;
    [SerializeField] string[] ammoList;
    [SerializeField] string[] hpList, knifeList, handgunList, shotgunList, machinegunList, rifleList, explosiveList;
    [SerializeField] string[] actionList1, actionList2, actionList3, actionList4, actionList5, actionList6, actionList7;
    [SerializeField] string[] extraList1, extraList2;
    [SerializeField] LinkedList<string[]> shopCardsList;
    [SerializeField] string[] startingDeckList, characterCardsList, mansionCardsList;
PhotonView view;

    private void Start()
    {
        isMaster = PhotonNetwork.IsMasterClient;
        buttonIsPressed = false;
        view = GetComponent<PhotonView>();

        shopDataFileNames = new string[17]
        {
        "AmmoCountList",
        "HPItemsList_AllCounted",
        "KnifeList_AllCounted",
        "HandgunsList_AllCounted",
        "ShotgunList_AllCounted",
        "AR_SG_List_AllCounted", //Machineguns
        "RifleList_AllCounted",
        "GrenadeList_AllCounted", //Explosives
        "ActionCardsList1_AllCounted",
        "ActionCardsList2_AllCounted",
        "ActionCardsList3_AllCounted",
        "ActionCardsList4_AllCounted",
        "ActionCardsList5_AllCounted",
        "ActionCardsList6_AllCounted",
        "ActionCardsList7_AllCounted",
        "Extra1List_AllCounted",
        "Extra2List_AllCounted"
        };
        Shop_CreateDataCards(1);
    }

    public void OnClickOverwriteNewFiles() //
    {
        if (isMaster && !buttonIsPressed)
        {
            Main_GetReadyForTheNextRoom();
            //OverwriteNewFiles();
        }      
    }
    public void Shop_CreateDataCards(int value)
    {

        string readFromFilePath = Application.persistentDataPath + "/Custom_data/ShopCardsData" + value + ".txt";
        fileLines = File.ReadAllLines(readFromFilePath).ToArray();
        //int[] countActionCards = Shop_ConvertTextToNumbers(fileLines[8]); //ALL ACTION CARDS
        //ammoList = GetIntArrayValuesFromLine(fileLines[0]); //Ammo
        int[] ammoCount = Shop_ConvertTextToNumbers(fileLines[0]);
        ammoList = new string[3];
        for (int i = 0; i < 3; i++)
        {
            ammoList[i] = ammoCount[i].ToString();
        }
        hpList = Shop_GetCardNamesByIndex(1);
        knifeList = Shop_GetCardNamesByIndex(2);
        handgunList = Shop_GetCardNamesByIndex(3);
        shotgunList = Shop_GetCardNamesByIndex(4);
        machinegunList = Shop_GetCardNamesByIndex(5);
        rifleList = Shop_GetCardNamesByIndex(6);
        explosiveList = Shop_GetCardNamesByIndex(7);
        actionList1 = Shop_GetCardNamesByIndex(8);
        actionList2 = Shop_GetCardNamesByIndex(9);
        actionList3 = Shop_GetCardNamesByIndex(10);
        actionList4 = Shop_GetCardNamesByIndex(11);
        actionList5 = Shop_GetCardNamesByIndex(12);
        actionList6 = Shop_GetCardNamesByIndex(13);
        actionList7 = Shop_GetCardNamesByIndex(14);
        extraList1 = Shop_GetCardNamesByIndex(15);
        extraList2 = Shop_GetCardNamesByIndex(16);


        shopCardsList = new LinkedList<string[]>();
        shopCardsList.AddLast(ammoList);
        shopCardsList.AddLast(hpList);
        shopCardsList.AddLast(knifeList);
        shopCardsList.AddLast(handgunList);
        shopCardsList.AddLast(shotgunList);
        shopCardsList.AddLast(machinegunList);
        shopCardsList.AddLast(rifleList);
        shopCardsList.AddLast(explosiveList);
        shopCardsList.AddLast(actionList1);
        shopCardsList.AddLast(actionList2);
        shopCardsList.AddLast(actionList3);
        shopCardsList.AddLast(actionList4);
        shopCardsList.AddLast(actionList5);
        shopCardsList.AddLast(actionList6);
        shopCardsList.AddLast(actionList7);
        shopCardsList.AddLast(extraList1);
        shopCardsList.AddLast(extraList2);
    }

    private string[] Shop_GetCardNamesByIndex(int index)
    {
        if (index <= 7)
        {
            int[] countArray = Shop_ConvertTextToNumbers(fileLines[index]); //Double digit values
            string[] cardNameArray = AllShopCards.GetComponent<AllShopCards>().GetShopCardByCount(countArray, index);
            cardNameArray = ShuffleArray(cardNameArray);
            return cardNameArray;
        }
        else if (index <=14) //Action cards
        {
            int[] countArray = Shop_ConvertTextToNumbers(fileLines[8]); //Double digit values
            string[] cardNameArray = AllShopCards.GetComponent<AllShopCards>().GetShopCardByCount(countArray, index);
            cardNameArray = ShuffleArray(cardNameArray);
            return cardNameArray;
        }
        else //Extra cards //index = 15-16
        {
            int line = index - 6;
            int[] countArray = Shop_ConvertTextToNumbers(fileLines[line]); //Double digit values
            string[] cardNameArray = AllShopCards.GetComponent<AllShopCards>().GetShopCardByCount(countArray, index);
            cardNameArray = ShuffleArray(cardNameArray);
            return cardNameArray;
        }
    }
    private int[] Shop_ConvertTextToNumbers(string s)
    {
        int index = 0;
        int lineLenght = s.Length;
        int[] cardType = new int[lineLenght / 2];
        int typeIndex = -1;

        for (int digit = 0; digit < lineLenght; digit++)
        {
            if (digit % 2 == 0 || digit == 0) //Even numbers
            {
                char firstCharacter = s[digit];
                typeIndex++;
                cardType[typeIndex] = (int)(firstCharacter - '0') * 10;
            }
            else //UnEven numbers
            {
                char lastCharacter = s[digit];
                cardType[typeIndex] += (int)(lastCharacter - '0');
            }
            index++;
        }
        return cardType;
    }
    private string[] ShuffleArray(string[] array)
    {
        string temp;

        string [] decklist = array;

        for (int i = 0; i < decklist.Length - 1; i++)
        {
            int rnd = Random.Range(i, decklist.Length);
            temp = decklist[rnd];
            decklist[rnd] = decklist[i];
            decklist[i] = temp;
        }
        return decklist;
    }

    private void Others_ReadTextFiles()
    {
        int value = GameStats.MansionDeckValue;
        if (value == 0)
            value = 1;

        startingDeckList = ReadFromCustomData("StartingDeckList");
        characterCardsList = ReadFromCustomData("CharacterList");
        mansionCardsList = ReadFromCustomData("MansionCards" + value);
    }
    private string[] ReadFromCustomData(string fileName)
    {
        string readFromFilePath = Application.streamingAssetsPath + "/Game_data/" + fileName + ".txt";
        string[] fileLines = File.ReadAllLines(readFromFilePath).ToArray();
        return fileLines;
    }


    private void Main_GetReadyForTheNextRoom()
    {
        Others_ReadTextFiles();
        SendAllDataToClients();
    }



    private void SendAllDataToClients() //And self
    {
        int index = 0; //Skip ammo card
        foreach (string[] array in shopCardsList)
        {
            view.RPC("Pun_OverwriteGameData", RpcTarget.AllBuffered, (object)array, shopDataFileNames[index]);
            index++;
        }

        view.RPC("Pun_OverwriteGameData", RpcTarget.AllBuffered, (object)startingDeckList, "StartingDeckList");
        view.RPC("Pun_OverwriteGameData", RpcTarget.AllBuffered, (object)characterCardsList, "CharacterList");
        view.RPC("Pun_OverwriteGameData", RpcTarget.AllBuffered, (object)mansionCardsList, "MatchMansionCards");

        GetComponent<LobbyRoomOpen>().TextFilesAreLoadedToOthers();
    }
    [PunRPC] void Pun_OverwriteGameData(string[] cardNames, string fileName)
    {
        WriteToGameDataFilePath(cardNames, fileName);
    }
    private void WriteToGameDataFilePath(string[] fileLines, string fileName)
    {
        string writeToFilePath = Application.streamingAssetsPath + "/Game_data/" + fileName + ".txt";
        File.WriteAllLines(writeToFilePath, fileLines);
        Debug.Log("Overwriting to: " + writeToFilePath);
    }


    /*
    private void OverwriteNewFiles()
    {
        buttonIsPressed = true;
        string[] shop_cards = new string[21]; //Add+1
        shop_cards[0] = "AmmoCountList"; shop_cards[1] = "ActionCardsList1_AllCounted"; shop_cards[2] = "ActionCardsList2_AllCounted";
        shop_cards[3] = "ActionCardsList3_AllCounted"; shop_cards[4] = "ActionCardsList4_AllCounted"; shop_cards[5] = "ActionCardsList5_AllCounted";
        shop_cards[6] = "ActionCardsList6_AllCounted"; shop_cards[7] = "ActionCardsList7_AllCounted"; 
        shop_cards[8] = "CharacterCustomlist"; shop_cards[9] = "AR_SG_List_AllCounted"; shop_cards[10] = "GrenadeList_AllCounted";
        shop_cards[11] = "HandgunsList_AllCounted"; shop_cards[12] = "HPItemsList_AllCounted"; shop_cards[13] = "KnifeList_AllCounted";
        shop_cards[14] = "RifleList_AllCounted"; shop_cards[15] = "ShotgunList_AllCounted"; 
        shop_cards[16] = "Extra1List_AllCounted"; shop_cards[17] = "Extra2List_AllCounted";

        //string[] other_cards = new string[3]; //Add+1
        shop_cards[18] = "StartingDeckList"; shop_cards[19] = "CharacterList";
        int i = GameStats.MansionDeckValue; i++;
        shop_cards[20] = "MansionCards" + i;


        //view.RPC("Pun_OverwriteShopCards", RpcTarget.OthersBuffered, (object)shop_cards);

        view.RPC("Pun_OverwriteNewFiles", RpcTarget.AllBuffered, (object)shop_cards);
        SendAllDataToClients();
    }
    [PunRPC]
    public void Pun_OverwriteShopCards(string[] shop_cards) //Overwrite from client's Custom_data folder to Game_data folder
    {
        int count = shop_cards.Length;
        for (int i = 0; i < count; i++) //Load Files from Game_data!
        {
            string textFile = shop_cards[i];
            //READ
            string readFromFilePath = Application.streamingAssetsPath + "/Game_data/" + textFile + ".txt";
            string[] fileLines = File.ReadAllLines(readFromFilePath).ToArray();

            if (i == 20) //Change "MansionCards1-4.txt" to "MatchMansionCards.txt" for "Game_Data" folder
                textFile = "MatchMansionCards";

            //WRITE
            string writeToFilePath = Application.streamingAssetsPath + "/Game_data/" + textFile + ".txt";
            File.WriteAllLines(writeToFilePath, fileLines);

        }

        GetComponent<LobbyRoomOpen>().TextFilesAreLoadedToOthers();

    }
    [PunRPC]
    public void Pun_OverwriteNewFiles(string[] other_cards) //Overwrite from client's Custom_data folder to Game_data folder
    {
        int count = other_cards.Length;
        for (int i = 0; i < count; i++) //Load Files from Custom_data!
        {
            string textFile = other_cards[i];
            //READ
            string readFromFilePath = Application.persistentDataPath + "/Custom_data/" + textFile + ".txt";
            string[] fileLines = File.ReadAllLines(readFromFilePath).ToArray();

            if (i == 20) //Change "MansionCards1-4.txt" to "MatchMansionCards.txt" for "Game_Data" folder
                textFile = "MatchMansionCards";

            //WRITE
            string writeToFilePath = Application.streamingAssetsPath + "/Game_data/" + textFile + ".txt";
            File.WriteAllLines(writeToFilePath, fileLines);

        }

        GetComponent<LobbyRoomOpen>().TextFilesAreLoadedToOthers();

    }
    */
}
