using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Photon.Pun;

//LobbyRoom.scene script
//Attach this script to object that contains "LobbyRoomOpen.cs" -script

public class OverwriteTextFileList : MonoBehaviourPun
{
    [SerializeField] GameObject AllShopCards;
    private GameObject MainCanvas;
    private bool isMaster,buttonIsPressed;
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
        MainCanvas = GameObject.FindWithTag("MainCanvas");
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
        //Shop_CreateDataCards(1); //false = testing        //NEW TEST 17.10.2022
    }

    public void OnClickOverwriteNewFiles() //LobbyRoom Start button -> When game is starting
    {
        if (isMaster && !buttonIsPressed)
        {
            Main_GetReadyForTheNextRoom();
            //OverwriteNewFiles();
        }      
    }
    [PunRPC] public void PUN_CreateAllShopDataCards(string[] masterFileLines)
    {
        //Send info that loading is happening in LobbyRoom
        MainCanvas.GetComponent<LobbyOptionsMenu>().SetMainInfoText("Game is starting...");
        
            fileLines = masterFileLines;

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
        

        int index = 0; //Skip ammo card
        foreach (string[] array in shopCardsList)
        {
            view.RPC("Pun_OverwriteGameData", RpcTarget.AllBuffered, (object)array, shopDataFileNames[index]);
            Pun_OverwriteGameData(array, shopDataFileNames[index]);
            index++;
        }
    }

    private string[] Shop_GetCardNamesByIndex(int index)
    {
        int[] countArray = Shop_ConvertTextToNumbers(fileLines[index]); //Double digit values
        string[] cardNameArray = AllShopCards.GetComponent<AllShopCards>().GetShopCardByCount(countArray, index);
        return cardNameArray;

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

    private void Others_ReadTextFiles()
    {
        int value = GameStats.MansionDeckValue;
        if (value == 0) //Avoid possible errors
            value = 1;
        //Debug.Log("MansionDeckValue=" + value);
        startingDeckList = ReadFromCustomData("StartingDeckList");
        characterCardsList = ReadFromCustomData("CharacterList");
        mansionCardsList = ReadFromCustomData("MansionCards" + value);
    }
    private string[] ReadFromCustomData(string fileName)
    {
        string readFromFilePath = Application.persistentDataPath + "/Custom_data/" + fileName + ".txt";
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
        int value = GameStats.ShopDeckDataValue;
        if (value == 0) value = 1;

        string readFromFilePath = Application.persistentDataPath + "/Custom_data/ShopCardsData" + value + ".txt";
        string[] masterFileLines = File.ReadAllLines(readFromFilePath).ToArray();
        masterFileLines = masterFileLines.Skip(1).ToArray(); //Skips the first line which contains the deck name (no values)     

        view.RPC("PUN_CreateAllShopDataCards", RpcTarget.AllBuffered, (object)masterFileLines);
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

}
