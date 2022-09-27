using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Photon.Pun;

//Attach this script to object that contains "LobbyRoomOpen.cs" -script

public class OverwriteTextFileList : MonoBehaviourPun
{
    private bool isMaster,buttonIsPressed;
    PhotonView view;

    private void Start()
    {
        isMaster = PhotonNetwork.IsMasterClient;
        buttonIsPressed = false;
        view = GetComponent<PhotonView>();
    }

    public void OnClickOverwriteNewFiles() //
    {
        if (isMaster && !buttonIsPressed)
        {          
            buttonIsPressed = true;
            string[] shop_cards = new string[21]; //Add+1
            shop_cards[0] = "AmmoCountList"; shop_cards[1] = "ActionCardsList1_AllCounted"; shop_cards[2] = "ActionCardsList2_AllCounted";
            shop_cards[3] = "ActionCardsList3_AllCounted"; shop_cards[4] = "ActionCardsList4_AllCounted"; shop_cards[5] = "ActionCardsList5_AllCounted";
            shop_cards[6] = "ActionCardsList6_AllCounted"; shop_cards[7] = "ActionCardsList7_AllCounted"; shop_cards[8] = "CharacterList";
            shop_cards[9] = "CharacterCustomlist"; shop_cards[10] = "AR_SG_List_AllCounted"; shop_cards[11] = "GrenadeList_AllCounted";
            shop_cards[12] = "HandgunsList_AllCounted"; shop_cards[13] = "HPItemsList_AllCounted"; shop_cards[14] = "KnifeList_AllCounted";
            shop_cards[15] = "RifleList_AllCounted"; shop_cards[16] = "ShotgunList_AllCounted"; shop_cards[17] = "StartingDeckList";
            shop_cards[18] = "Extra1List_AllCounted"; shop_cards[19] = "Extra2List_AllCounted";
            int i = GameStats.MansionDeckValue; i++;
            string mansionCards = "MansionCards" + i;
            shop_cards[20] = mansionCards;

            view.RPC("Pun_OverwriteNewFiles", RpcTarget.AllBuffered, (object)shop_cards);
            //Pun_OverwriteNewFiles(shop_cards);
        }      
    }

    [PunRPC]
    public void Pun_OverwriteNewFiles(string[] shop_cards) //Overwrite from client's Custom_data folder to Game_data folder
    {

        foreach (string textFile in shop_cards)
        {
            string readFromFilePath = Application.persistentDataPath + "/Custom_data/" + textFile + ".txt";
            string[] fileLines = File.ReadAllLines(readFromFilePath).ToArray();

            //string writeToFilePath = Application.persistentDataPath + "/Game_data/" + textFile + ".txt";
            string writeToFilePath = Application.streamingAssetsPath + "/Game_data/" + textFile + ".txt";
            File.WriteAllLines(writeToFilePath, fileLines);

        }

        GetComponent<LobbyRoomOpen>().TextFilesAreLoadedToOthers();

    }
    
}
